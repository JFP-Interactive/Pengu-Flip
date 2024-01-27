using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFeather : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProgressManager.Instance.Feathers++;
            Destroy(gameObject);
        }
    }
}