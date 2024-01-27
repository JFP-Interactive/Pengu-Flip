using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProgressManager.instance.fish++;
            Destroy(gameObject);
        }
    }
}