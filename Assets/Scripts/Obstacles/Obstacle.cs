using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool dontChangeRotation;
    
    public IEnumerator OnPlaced(GameObject target)
    {
        if (target == null) yield break;
        yield return new WaitUntil(() => Vector3.Distance(target.transform.position, transform.position) > 80f);
        if (gameObject.IsDestroyed()) yield break;
        Destroy(gameObject);
    }
}
