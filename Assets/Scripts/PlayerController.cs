using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _climbSpeed = 10f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;
    private CapsuleCollider2D _cc2d;
    private BoxCollider2D _bc2d;
    [SerializeField] private float maxJumpHeight = 3f;

    private float startingYposition;
    private float baseGravity;
    public bool pressedJump = false;
    public bool releasedJump = false;

    //private GroundDetector _groundedDetector;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();        
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _cc2d = GetComponent<CapsuleCollider2D>();
        _bc2d = GetComponentInChildren<BoxCollider2D>();
        baseGravity = _rb.gravityScale;
        //_groundedDetector = GetComponentInChildren<GroundDetector>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            pressedJump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            releasedJump = true;
        }

        ClimbLadder();
    }
    void FixedUpdate()
    {
        //Left Right
        _rb.velocityX = Input.GetAxis("Horizontal") * _moveSpeed * Time.fixedDeltaTime;

        if (Input.GetAxis("Horizontal") * _moveSpeed < Mathf.Epsilon*-1)
        {
            _sr.flipX = true;
            _animator.SetBool("isRunning", true);
        }
        else if (Input.GetAxis("Horizontal") * _moveSpeed > Mathf.Epsilon)
        {
            _animator.SetBool("isRunning", true);
            _sr.flipX = false;
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
       
        //Jump
        if (pressedJump) 
        { 
            StartJump();
        }
        else if (releasedJump || (transform.position.y - startingYposition) > maxJumpHeight)
        {
            pressedJump = false;
            StopJump();
        }
        else
        {
            pressedJump = false;
        }
    }
    private void StartJump()
    {
        if (!_bc2d.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
            return;
        }
        startingYposition = transform.position.y;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f); // Reset vertical velocity before jumping
        _rb.AddForce(new Vector2(0,_jumpForce), ForceMode2D.Impulse);
        pressedJump = false;
    }

    private void StopJump()
    {
        releasedJump = false;
    }

    private void ClimbLadder()
    {
        //Make it so the player ignores the ladder if not pressing up or down
        var playerMovingVertically = Mathf.Abs(_rb.velocityY) > Mathf.Epsilon;
        if (!_bc2d.IsTouchingLayers(LayerMask.GetMask("Interactive")))
        {
            _rb.gravityScale = baseGravity;
            _animator.SetBool("isClimbing", false);
            return;
        }
        else if (_bc2d.IsTouchingLayers(LayerMask.GetMask("Interactive")))
        {
            _animator.SetBool("isClimbing", true);
            _animator.SetFloat("ClimbSpeed", !playerMovingVertically ? 0f : 1f);
            _rb.velocityY = Input.GetAxis("Vertical") * _climbSpeed * Time.fixedDeltaTime;
            _rb.gravityScale = 0; 
            pressedJump = false;
        }

        
    }
}
