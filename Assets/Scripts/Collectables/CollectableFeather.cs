using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFeather : MonoBehaviour
{
    private ProgressManager progressManager;
    private FeatherCounterScript featherCounterScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            progressManager = ProgressManager.instance;
            featherCounterScript = FeatherCounterScript.instance;
            progressManager.feathers++;
            featherCounterScript.UpdateFeatherCounter(progressManager.feathers);
            Destroy(gameObject);
        }
    }
}