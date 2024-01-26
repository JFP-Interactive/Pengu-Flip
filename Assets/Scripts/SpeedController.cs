using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Rigidbody physicRigidbody;
    [SerializeField, Range(1,100)] private float maxSpeed = 10f;
    
    void Update()
    {
        if (physicRigidbody == null) return;
        var velocity = physicRigidbody.velocity;
        if (velocity.magnitude > maxSpeed)
        {
            physicRigidbody.velocity = velocity.normalized * maxSpeed;
        }
    }
}
