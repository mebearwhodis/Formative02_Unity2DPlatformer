using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchInteraction : MonoBehaviour
{
    //TODO: Add a "X" Button over switch, setActive when player is in the hitbox
    private Animator _animator;
    private GameObject _levelExit;
    private bool _playerInRange = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _levelExit = GameObject.FindWithTag("Exit");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && _playerInRange)
        {
            _animator.SetTrigger("UseSwitch");
            Animator otherAnimator = _levelExit.gameObject.GetComponent<Animator>();
            otherAnimator.SetBool("ExitOpen", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _playerInRange = true;
    }   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _playerInRange = false;
    }
}
