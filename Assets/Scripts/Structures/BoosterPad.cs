using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Rigidbody>();
        SpeedController.Instance.maxSpeed = 20f;
        player.AddForce(transform.forward * 1000f);
        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(2f);
        SpeedController.Instance.maxSpeed = SpeedController.Instance.standardMaxSpeed;
    }
}
