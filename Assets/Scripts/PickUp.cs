using System;
using Mono.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickup;
    public HUD _hud;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if(!other.GetComponent<PlayerController>().isAlive) return;
        _hud = FindAnyObjectByType<HUD>();
        AudioSource.PlayClipAtPoint(_coinPickup, Camera.main.transform.position);
        gameObject.SetActive(false);
        _hud.AddTime(10);
        Destroy(gameObject);
        
    }
}