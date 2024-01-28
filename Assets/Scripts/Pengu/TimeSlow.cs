using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeSlow : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.05f;
    [SerializeField] private float fillSpeed = 0.05f;
    [SerializeField] private float slowDownLength = 2f;
    [SerializeField] private Slider slider;
    [SerializeField] private Volume volume;
    [SerializeField, Range(0, 1)] private float vignetteIntensityNormal;
    [SerializeField, Range(0, 1)] private float vignetteIntensityTimeSlow;
    private Vignette vignette;

    private float _currentSlowDownLeft;
    private bool _isSlowingDown;

    private void Start()
    {
        _currentSlowDownLeft = slowDownLength;
        vignette = (Vignette)volume.profile.components[2];
        vignette.intensity.value = vignetteIntensityNormal;
    }
    
    public void SlowDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSlowingDown = true;
            Debug.Log("Vignette Slow");
            vignette.intensity.value = vignetteIntensityTimeSlow;
        }
        else if (context.canceled)
        {
            _isSlowingDown = false;
            Debug.Log("Vignette Normal");
            vignette.intensity.value = vignetteIntensityNormal;
        }
    }
    
    private void Update()
    {
        if (Time.timeScale == 0) {
            return;
        }
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