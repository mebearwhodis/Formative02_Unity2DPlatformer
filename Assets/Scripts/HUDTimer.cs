using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    
    public float _timeRemaining = 10;
    public bool _timerOn = false;
    void Start()
    {
        _timerOn = true;
    }

    private void Update()
    {
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
                SceneManager.LoadScene(0);
            }
        }
    }
    void DisplayTimer(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timerText.text = seconds.ToString();
    }

}