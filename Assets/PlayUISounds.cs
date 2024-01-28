using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayUISounds : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private EventSystem eventSystem;
    
    private GameObject _lastSelectedObject;
    
    private void Start()
    {
        eventSystem = EventSystem.current;
    }
    
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            if (eventSystem.currentSelectedGameObject != _lastSelectedObject)
            {
                PlaySound();
                _lastSelectedObject = eventSystem.currentSelectedGameObject;
            }
        }
        else
        {
            _lastSelectedObject = null;
        }
    }
    
    public void PlaySound()
    {
        Debug.Log("PlaySound");
        audioSource.PlayOneShot(buttonSound);
    }
}
