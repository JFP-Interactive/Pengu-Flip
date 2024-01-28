using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Rigidbody physicRigidbody;
    [SerializeField, Range(1,100)] public float maxSpeed = 10f;
    [SerializeField, Range(0, 5)] public float minSpeed = 0.5f;
    [SerializeField] private float deathDelayInSeconds = 3;
    [SerializeField] private Volume volume;
    private GameObject deathMenu;
    private GameObject ingameUI;
    [SerializeField] private UnityEvent OnDeath = new ();
    [SerializeField] private UnityEvent OnStart = new ();
    private Vignette vignette;
    private Color vignetteColorOriginal;
    
    private int currentDeathDelay = 0;
    public static SpeedController Instance { get; private set; }
    public float standardMaxSpeed = 10f;
    
    private void Start()
    {
        Instance = this;
        standardMaxSpeed = maxSpeed;
        deathMenu = GameObject.Find("DeathMenu");
        deathMenu.SetActive(false);
        ingameUI = GameObject.Find("IngameMenu");
        OnStart.Invoke();
        vignette = (Vignette)volume.profile.components[2];
        vignetteColorOriginal = vignette.color.value;
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
            vignette.color.value = Color.red;
            currentDeathDelay++;
            if (currentDeathDelay >= deathDelayInSeconds / Time.fixedDeltaTime)
            {
                Die();
            }
        }
        else
        {
            currentDeathDelay = 0;
            vignette.color.value = vignetteColorOriginal;
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
        HighScoreManager.instance.SetHighScore();
        Time.timeScale = 0f;
        ingameUI.SetActive(false);
        deathMenu.SetActive(true);
    }
}