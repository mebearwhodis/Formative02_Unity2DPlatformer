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
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private CapsuleCollider2D _cc2d;
    [SerializeField] private float _respawnX;
    [SerializeField] private float _respawnY;
    [SerializeField] private Vector2 deathKnockback = new Vector2 (10f, 10f);
    
    
    public bool pressedJump = false;
    public bool releasedJump = false;
    private bool isAlive = true;
    private float startingYposition;
    private float baseGravity;
    private Vector2 _startingPos;
    private Vector2 _respawnPos;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc2d;

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
        _respawnPos = new Vector2(_respawnX, _respawnY); 
        
        if (!isAlive) {return;}
        
        if (Input.GetButtonDown("Jump"))
        {
            pressedJump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            releasedJump = true;
        }

        ClimbLadder();
        Die();
    }
    void FixedUpdate()
    {
        if (!isAlive) {return;}
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
            pressedJump = false;
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

    private void Die()
    {
        if (_cc2d.IsTouchingLayers(LayerMask.GetMask("Enemies", "Traps")) && !_bc2d.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            isAlive = false;
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        _animator.SetTrigger("isDying");
        _rb.velocity = deathKnockback; 
        
        yield return new WaitForSeconds (.75f);
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(0, 0);
        
        yield return new WaitForSeconds (1);
        _rb.transform.position = _respawnPos;
        isAlive = true;
        _rb.gravityScale = baseGravity;
        _animator.SetTrigger("hasRespawned");
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Deadly"))
    //     {
    //         _rb.transform.position = _respawnPos;
    //         isAlive = true;
    //     }
    // }
}
