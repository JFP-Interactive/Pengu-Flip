using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Rigidbody physicRigidbody;
    [SerializeField, Range(1,100)] public float maxSpeed = 10f;
    [SerializeField, Range(0, 5)] public float minSpeed = 0.5f;
    [SerializeField] private int deathDelayInSeconds = 3;
    [SerializeField] GameObject deathMenu;
    private int currentDeathDelay = 0;
    public static SpeedController Instance { get; private set; }
    public float standardMaxSpeed = 10f;
    
    private void Start()
    {
        Instance = this;
        standardMaxSpeed = maxSpeed;
    }

    void Update()
    {
        if (physicRigidbody == null) return;
        var velocity = physicRigidbody.velocity;
        if (velocity.magnitude > maxSpeed)
        {
            physicRigidbody.velocity = velocity.normalized * maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (physicRigidbody == null) return;
        var velocity = physicRigidbody.velocity;
        if (velocity.magnitude <= minSpeed)
        {
            currentDeathDelay++;
            if (currentDeathDelay >= deathDelayInSeconds / Time.fixedDeltaTime)
            {
                Time.timeScale = 0f;
                deathMenu.SetActive(true);
            }
        }
        else
        {
            currentDeathDelay = 0;
        }
    }
}