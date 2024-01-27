using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimeSlow : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.05f;
    [SerializeField] private float fillSpeed = 0.05f;
    [SerializeField] private float slowDownLength = 2f;
    [SerializeField] private Slider slider;

    private float _currentSlowDownLeft;
    private bool _isSlowingDown;

    private void Start()
    {
        _currentSlowDownLeft = slowDownLength;
    }
    
    public void SlowDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSlowingDown = true;
        }
        else if (context.canceled)
        {
            _isSlowingDown = false;
        }
    }
    
    private void Update()
    {
        if (_isSlowingDown)
        {
            if (_currentSlowDownLeft <= 0)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                _isSlowingDown = false;
            }
            else
            {
                Time.timeScale = slowDownFactor;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                _currentSlowDownLeft -= Time.deltaTime;
            }
        }
        else
        {
            if (_currentSlowDownLeft < slowDownLength)
            {
                _currentSlowDownLeft += Time.deltaTime * fillSpeed;
            }
            else
            {
                _currentSlowDownLeft = slowDownLength;
            }
            Time.timeScale = 1f;
        }
        
        slider.value = _currentSlowDownLeft / slowDownLength;
    }
}
