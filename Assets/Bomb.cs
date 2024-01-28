using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float bombStrength = 10000f;
    [SerializeField] private ParticleSystem explosionParticles;
    public UnityEvent BombExploded = new UnityEvent();
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Rigidbody>();
        SpeedController.Instance.maxSpeed = 100f;
        player.velocity = Vector3.zero;
        var direction = player.transform.position - transform.position;
        player.AddForce(direction.normalized * bombStrength);
        player.AddForce(Vector3.up * bombStrength);
        BombExploded.Invoke();

        if (explosionParticles != null)
        {
            ParticleSystem explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            explosion.Play();

            Destroy(explosion.gameObject, explosion.main.duration);
        }

        StartCoroutine(ResetSpeed());
        Destroy(gameObject);
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(2f);
        SpeedController.Instance.maxSpeed = SpeedController.Instance.standardMaxSpeed;
    }
}