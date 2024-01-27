using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    //[SerializeField] private Effect[] effects;
    [SerializeField, Range(0,180)] public float maxRotation = 45f;
    [SerializeField] public Vector3 rotationOffset;

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    public void Place()
    {
        var clone = Instantiate(gameObject);
        clone.transform.position = transform.position;
        clone.transform.rotation = transform.rotation;
        var newStructure = clone.GetComponent<Structure>();
        newStructure.enabled = false;
        clone.GetComponent<Collider>().enabled = true;
        newStructure.OnPlaced();
    }

    private void OnPlaced()
    {
        //foreach (var effect in effects)
        //{
        //    effect.Fire();
        //}
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);
    }
}