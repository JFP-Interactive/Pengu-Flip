using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithTarget : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private bool moveX;
    [SerializeField] private bool moveY;
    [SerializeField] private bool moveZ;
    
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - target.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        var targetPosition = target.transform.position;
        transform.position = new Vector3(moveX ? targetPosition.x + _offset.x : position.x, moveY ? targetPosition.y + _offset.y : position.y, moveZ ? targetPosition.z + _offset.z : position.z);
    }
}
