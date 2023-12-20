using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _cc2d;
    private BoxCollider2D _bc2d;
    private Light2D _light;
    private Animator _animator;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _cc2d = GetComponent<CapsuleCollider2D>();
        _bc2d = GetComponent<BoxCollider2D>();
        _light = GetComponent<Light2D>();
        _animator.SetBool("isWalking", true);
    }

    void Update()
    {
        MoveHorizontally();
        _rb.velocity = new Vector2(_moveSpeed, 0f);
        GetHit();
    }
    
    private void MoveHorizontally()
    {
        if (!_bc2d.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
            _moveSpeed = -_moveSpeed;
            transform.localScale = new Vector2(-(Mathf.Sign(_rb.velocityX)), 1f);
        }
    }
    
    private void GetHit()
    {
        if (_cc2d.IsTouchingLayers(LayerMask.GetMask("Feet")))
        {
            _animator.SetBool("isWalking", false);
            _animator.SetTrigger("Hit");
            _moveSpeed = 0;
            _cc2d.enabled = false;
            _bc2d.enabled = false;
            _light.intensity = 0.5f;
        }
    }
}
