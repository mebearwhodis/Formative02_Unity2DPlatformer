using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDungeon : MonoBehaviour
{
 private Animator _animator;

 void Start()
 {
  _animator = GetComponent<Animator>();
 }
 private void OnTriggerEnter2D(Collider2D other)
 {
  if (other.gameObject.CompareTag("Player") && _animator.GetBool("ExitOpen"))
  {
   int currentScene = SceneManager.GetActiveScene().buildIndex;
   //Going to Win Screen, modify in case of multiple levels
   SceneManager.LoadScene(currentScene + 2);
  }
 }
}
