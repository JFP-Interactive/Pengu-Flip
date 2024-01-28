using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherCounterScript : MonoBehaviour
{
    public static FeatherCounterScript instance;
    private TMPro.TMP_Text text;

    private void Awake()
    {
        instance = this;
        text = GetComponent<TMPro.TMP_Text>();
    }

    private void Start()
    {
        UpdateFeatherCounter(0);
    }

    public void UpdateFeatherCounter(int featherCount)
    {
        text.text = featherCount.ToString();
    }
}