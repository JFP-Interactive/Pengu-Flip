using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFish : MonoBehaviour
{
    [SerializeField] int pointsPerFish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HighScoreManager.instance.GivePoints(pointsPerFish);
            Destroy(gameObject);
        }
    }
}