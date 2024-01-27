using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }
    public int Fish = 0;
    public int Feathers = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        ResetTemporaryValues();
    }

    private void Update()
    {
        print("Fish: " + Fish + "   Feathers: " + Feathers);
    }

    private void ResetTemporaryValues()
    {
        Instance.Feathers = 0;
    }
}