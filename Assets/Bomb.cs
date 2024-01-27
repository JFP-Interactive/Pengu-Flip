using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float bombStrength = 10000f;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Rigidbody>();
        SpeedController.Instance.maxSpeed = 100f;
        //boost away from bomb
        player.velocity = Vector3.zero;
        var direction = player.transform.position - transform.position;
        player.AddForce(direction.normalized * bombStrength);
        player.AddForce(Vector3.up * bombStrength);
        StartCoroutine(ResetSpeed());
        Destroy(gameObject);
    }
    
    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(2f);
        SpeedController.Instance.maxSpeed = SpeedController.Instance.standardMaxSpeed;
    }
}
