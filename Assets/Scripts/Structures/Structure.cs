using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    //[SerializeField] private Effect[] effects;
    [SerializeField, Range(0, 180)] public float maxRotation = 45f;
    [SerializeField] public Vector3 rotationOffset;
    [SerializeField] private LayerMask blockingLayer;

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    public bool Place()
    {
        if (!CheckPlaceable()) return false;
        var clone = Instantiate(gameObject, transform.position, transform.rotation);
        var newStructure = clone.GetComponent<Structure>();
        newStructure.enabled = false;
        clone.GetComponent<Collider>().enabled = true;
        StartCoroutine(newStructure.OnPlaced());
        return true;
    }

    public bool CheckPlaceable()
    {
        //check if another structure is in the colliders way
        var size = transform.lossyScale;
        var colliders = Physics.OverlapBox(transform.position, size / 2, transform.rotation, blockingLayer);
        return colliders.Length == 0;
    }

    private IEnumerator OnPlaced()
    {
        //foreach (var effect in effects)
        //{
        //    effect.Fire();
        //}
        var target = MoveWithTarget.Instance.gameObject;
        yield return new WaitUntil(() => Vector3.Distance(target.transform.position, transform.position) > 80f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);
    }
}