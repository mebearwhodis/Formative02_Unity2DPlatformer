using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _climbSpeed = 10f;
    [SerializeField] private float _maxJumpHeight = 3f;
    [SerializeField] private Vector2 _deathKnockback = new Vector2 (10f, 10f);
    
    public int _playerLives = 3;
    public bool pressedJump = false;
    public bool releasedJump = false;
    public bool isAlive = true;
    private float startingYposition;
    [SerializeField] private float baseGravity;
    private Vector2 _startingPos;
    private Vector2 _respawnPos;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc2d;
    private CapsuleCollider2D _cc2d;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();        
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _cc2d = GetComponent<CapsuleCollider2D>();
        _bc2d = GetComponentInChildren<BoxCollider2D>();
        baseGravity = _rb.gravityScale;
        _playerLives = 5;
    }

    // Update is called once per frame
    private void Update()
    {
        
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
        else if (releasedJump || (transform.position.y - startingYposition) > _maxJumpHeight)
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
        //Make it so the player ignores the ladder if not pressing up or down??
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            Animator _otherAnimator = other.gameObject.GetComponent<Animator>();
            _otherAnimator.SetTrigger("ActivateCheckpoint");
            Vector2 checkpointPos =
                new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y);
            _respawnPos = checkpointPos;
        }

        if (other.gameObject.CompareTag("Ladder"))
        {
            var playerMovingVertically = Mathf.Abs(_rb.velocityY) > Mathf.Epsilon;
            _animator.SetBool("isClimbing", true);
            _animator.SetFloat("ClimbSpeed", !playerMovingVertically ? 0f : 1f);
            _rb.velocityY = Input.GetAxis("Vertical") * _climbSpeed * Time.fixedDeltaTime;
            _rb.gravityScale = 0;
            pressedJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            _rb.gravityScale = baseGravity;
            _animator.SetBool("isClimbing", false);
            return;
        }
    }
    
    private void Die()
    {
        if (_cc2d.IsTouchingLayers(LayerMask.GetMask("Enemies", "Traps")) /* && !_bc2d.IsTouchingLayers(LayerMask.GetMask("Enemies"))*/)
        {
            if (_playerLives > 0)
            {
                _playerLives--;
                isAlive = false;
                StartCoroutine(Respawn());
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    private IEnumerator Respawn()
    {
        _animator.SetTrigger("isDying");
        _deathKnockback.x *= Mathf.Sign(_rb.velocityX);
        _rb.velocity = _deathKnockback; 
        
        yield return new WaitForSeconds (.65f);
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(0, 0);
        
        yield return new WaitForSeconds (1);
        //_rb.velocity = new Vector2(0, 0);
        _rb.transform.position = _respawnPos;
        isAlive = true;
        _rb.gravityScale = baseGravity;
        _animator.SetTrigger("hasRespawned");
    }

}
