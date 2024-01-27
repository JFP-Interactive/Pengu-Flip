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
            ProgressManager.Instance.Fish++;
            Destroy(gameObject);
        }
    }
}