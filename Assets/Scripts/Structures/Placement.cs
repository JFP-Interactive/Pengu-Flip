using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Structure currentStructure;
    [SerializeField] private float rotationSpeed = .1f;
    
    private Camera _camera;
    private float _yRotation;
    private RaycastHit _hit;

    private void Start()
    {
        _camera = Camera.main;
        SetObject(currentStructure);
    }

    private void FixedUpdate()
    {
        if (currentStructure == null) return;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out _hit, 100f, groundLayer))
        {
            currentStructure.gameObject.SetActive(true);
        }
        else
        {
            currentStructure.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentStructure == null) return;
        _yRotation += Mouse.current.scroll.ReadValue().y * rotationSpeed;
        var maxRotation = currentStructure.maxRotation;
        _yRotation = Mathf.Clamp(_yRotation, -maxRotation, maxRotation);
        currentStructure.transform.position = _hit.point;
        currentStructure.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal) * Quaternion.Euler(0, _yRotation, 0) * Quaternion.Euler(currentStructure.rotationOffset);
        currentStructure.transform.localPosition += currentStructure.transform.up * currentStructure.transform.localScale.y / 2;
    }

    public void SetObject(Structure structure)
    {
        if (currentStructure != null) Destroy(currentStructure.gameObject);
        currentStructure = Instantiate(structure);
    }

    public void Place(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled || currentStructure == null || !currentStructure.gameObject.activeSelf) return;
        currentStructure.Place();
    }
    
    public void ResetRotation(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        _yRotation = 0f;
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