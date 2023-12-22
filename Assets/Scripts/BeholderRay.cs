using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderRay : MonoBehaviour
{
    private BoxCollider2D _bc2d;
    private SpriteRenderer _sr;
    private Animator _animator;

    private void Start()
    {
        _bc2d = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animator.GetBool("isFiring"))
        {
            _bc2d.enabled = true;
            _sr.enabled = true;
        }
        else
        {
            _bc2d.enabled = false;
            _sr.enabled = false;
        }
    }
}
