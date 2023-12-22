using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderAttack : MonoBehaviour
{
    private BoxCollider2D _bc2d;
    private SpriteRenderer _sr;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(BeholderRay());
    }

    private IEnumerator BeholderRay()
    {
        while (true)
        {
            _animator.SetBool("isFiring", false);

            yield return new WaitForSecondsRealtime(2.499f);

            _animator.SetBool("isFiring", true);

            yield return new WaitForSecondsRealtime(2.417f);
        }
    }
}