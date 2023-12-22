using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIP : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("isDying");
    }
}