using UnityEngine;

public class StayWithLook : MonoBehaviour
{
    [SerializeField] private GameObject physicGameObject;
    [SerializeField] private GameObject rotationGameObject;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField, Range(1, 100)] private float rotationSpeed = 10f;

    private Quaternion _nextRotation;
    private Rigidbody _physicRigidbody;

#if UNITY_EDITOR
    //if rotationOffset is changed in editor updated transforms rotation
    private void OnValidate()
    {
        if (rotationGameObject == null) return;
        rotationGameObject.transform.GetChild(0).rotation = Quaternion.Euler(rotationOffset);
        rotationGameObject.transform.GetChild(0).localPosition = positionOffset;
    }
#endif
    
    // Start is called before the first frame update
    void Start()
    {
        _physicRigidbody = physicGameObject.GetComponent<Rigidbody>();
        _nextRotation = rotationGameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (physicGameObject == null || rotationGameObject == null) return;
        var targetTransform = physicGameObject.transform;
        var selfTransform = rotationGameObject.transform;
        selfTransform.position = targetTransform.position;
        var velocity = _physicRigidbody.velocity;
        if (velocity.magnitude < 0.1f) return;
        _nextRotation = Quaternion.LookRotation(velocity);
        selfTransform.rotation =
            Quaternion.Slerp(selfTransform.rotation, _nextRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmos()
    {
        if (_physicRigidbody == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(physicGameObject.transform.position, physicGameObject.transform.position + _physicRigidbody.velocity);
    }
}