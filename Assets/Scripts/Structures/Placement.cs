using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Structure currentStructure;
    [SerializeField] private float rotationSpeed = 10f;
    
    private Camera _camera;
    private float yRotation = 0f;

    private void Start()
    {
        _camera = Camera.main;
        SetObject(currentStructure);
    }

    private void Update()
    {
        if (currentStructure == null) return;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 100f, groundLayer))
        {
            currentStructure.gameObject.SetActive(true);
            currentStructure.transform.position = hit.point;
            yRotation += Mouse.current.scroll.ReadValue().y * rotationSpeed;
            var maxRotation = currentStructure.maxRotation;
            yRotation = Mathf.Clamp(yRotation, -maxRotation, maxRotation);
            currentStructure.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0, yRotation, 0);
        }
        else
        {
            currentStructure.gameObject.SetActive(false);
        }
    }

    public void SetObject(Structure structure)
    {
        if (currentStructure != null) Destroy(currentStructure.gameObject);
        currentStructure = Instantiate(structure);
    }

    public void Place(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        currentStructure.Place();
    }
    
    public void ResetRotation(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        yRotation = 0f;
    }

    public void Cancel()
    {
        // Implement cancellation logic if necessary
    }

    public void OnDrawGizmos()
    {
        if (currentStructure == null) return;
        if (_camera == null) _camera = Camera.main;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 1000f, groundLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.5f);
        }
    }
}