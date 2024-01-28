using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Rigidbody physicRigidbody;
    [SerializeField, Range(1,100)] public float maxSpeed = 10f;
    [SerializeField, Range(0, 5)] public float minSpeed = 0.5f;
    [SerializeField] private int deathDelayInSeconds = 3;
    private GameObject deathMenu;
    private GameObject ingameUI;
    
    private int currentDeathDelay = 0;
    public static SpeedController Instance { get; private set; }
    public float standardMaxSpeed = 10f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip; 
    private float originalPitch;

    [SerializeField, Range(0, 1)] private float maxVolume = 0.5f; // Max Lautstärke bei MaxSpeed
    [SerializeField, Range(0, 1)] private float minVolume = 0.1f; // Min Lautstärke bei MinSpeed
    [SerializeField, Range(-1, 1)] private float pitchChangePercent = 0.1f; // Prozentsatz der Pitch-Änderung

    private void Start()
    {
        Instance = this;
        standardMaxSpeed = maxSpeed;
        deathMenu = GameObject.Find("DeathMenu");
        deathMenu.SetActive(false);
        ingameUI = GameObject.Find("IngameMenu");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();

        originalPitch = audioSource.pitch;
    }

    void Update()
    {
        if (physicRigidbody == null) return;
        var velocity = physicRigidbody.velocity;

        float volume = Mathf.Lerp(minVolume, maxVolume, velocity.magnitude / maxSpeed);
        audioSource.volume = volume;

        float speedPercent = velocity.magnitude / maxSpeed;
        float pitchChange = pitchChangePercent * speedPercent;
        audioSource.pitch = originalPitch + pitchChange;

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
                Die();
            }
        }
        else
        {
            currentDeathDelay = 0;
        }
    }

    private void Die()
    {
        HighScoreManager.instance.SetHighScore();
        Time.timeScale = 0f;
        ingameUI.SetActive(false);
        deathMenu.SetActive(true);
    }
}