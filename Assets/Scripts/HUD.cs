using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _timerText;
    
    private GameObject _player;
    public float _timeRemaining = 10;
    public bool _timerOn = false;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _timerOn = true;
    }

    private void Update()
    {
        _livesText.text = _player.gameObject.GetComponent<PlayerController>()._playerLives.ToString() + "♥";
        if (_timerOn)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                DisplayTimer(_timeRemaining);
            }
            else
            {
                _timeRemaining = 0;
                _timerOn = false;
            }
        }
    }
    void DisplayTimer(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(int amount)
    {
        _timeRemaining += amount;
    }
}