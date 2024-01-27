using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpController : MonoBehaviour
{
    ProgressManager progressManager;
    [SerializeField] Rigidbody playerRigidBody;
    [SerializeField] float flapHeight = 10;

    private void Awake()
    {
        progressManager = ProgressManager.Instance;
        FeatherCounterScript.instance.GetComponent<TMPro.TMP_Text>().text = progressManager.Feathers.ToString();
    }
    public void UsePowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && progressManager.Feathers >= 1)
        {
            playerRigidBody.velocity += Vector3.up * flapHeight;
            progressManager.Feathers--;
        }
    }
}