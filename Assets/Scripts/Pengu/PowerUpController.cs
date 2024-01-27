using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpController : MonoBehaviour
{
    ProgressManager progressManager;
    FeatherCounterScript featherCounterScript;
    [SerializeField] Rigidbody playerRigidBody;
    [SerializeField] float flapHeight = 10;

    private void Awake()
    {
        progressManager = ProgressManager.instance;
        featherCounterScript = FeatherCounterScript.instance;
        featherCounterScript.GetComponent<TMPro.TMP_Text>().text = progressManager.feathers.ToString();
    }
    public void UsePowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && progressManager.feathers >= 1)
        {
            playerRigidBody.velocity += Vector3.up * flapHeight;
            progressManager.feathers--;
            featherCounterScript.UpdateFeatherCounter(progressManager.feathers);
        }
    }
}