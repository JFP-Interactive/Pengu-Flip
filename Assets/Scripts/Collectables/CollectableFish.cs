using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableFish : MonoBehaviour
{
    [SerializeField] int pointsPerFish;

    public UnityEvent FishEaten = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HighScoreManager.instance.GivePoints(pointsPerFish);
            FishEaten.Invoke();
            Destroy(gameObject);
        }
    }
}