using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;
    [SerializeField] private CapsuleCollider2D _cc2d;
    private BoxCollider2D _bc2d;
    private Vector2 _startingPos;
    [SerializeField] private float _respawnX;
    [SerializeField] private float _respawnY;
    private Vector2 _respawnPos;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();        
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _bc2d = GetComponent<BoxCollider2D>();
        _startingPos = new Vector2(0, 0);
        _respawnPos = _startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        _respawnPos = new Vector2(_respawnX, _respawnY);  
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly"))
        {
            _rb.transform.position = _respawnPos;
        }
    }
  
}
