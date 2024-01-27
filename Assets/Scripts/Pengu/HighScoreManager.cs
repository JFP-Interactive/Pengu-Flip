using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] int meterPerPoint = 10;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private Transform playerTransform;
    private int currentScore = 0;

    private void FixedUpdate()
    {
        currentScore = Mathf.Max(currentScore, (int)playerTransform.position.z / meterPerPoint);
        scoreText.text = currentScore.ToString();
    }
}