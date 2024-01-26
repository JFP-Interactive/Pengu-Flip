using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject physicGameObject;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector3 positionOffset;

    private Vector3 _startPosition;
    private void OnValidate()
    {
        if (physicGameObject == null) return;
        transform.LookAt(physicGameObject.transform);
        transform.rotation = Quaternion.Euler(rotationOffset) * transform.rotation;
        transform.position = physicGameObject.transform.position + positionOffset;
    }

    private void Start()
    {
        _startPosition = transform.position - physicGameObject.transform.position;
    }

    private void Update()
    {
        if (physicGameObject == null) return;
        var position = physicGameObject.transform.position + _startPosition;
        position.x = _startPosition.x;
        transform.position = position;
    }
}
