using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    private Rigidbody2D _rb;
    private Animator _animator;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _rb.velocity = new Vector2(_moveSpeed, 0f);
        _animator.SetBool("isWalking", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _moveSpeed = -_moveSpeed;
        transform.localScale = new Vector2(-(Mathf.Sign(_rb.velocityX)), 1f);
    }
}
