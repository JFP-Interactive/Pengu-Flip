using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsEvents : MonoBehaviour
{
    public static UnityEvent OnHighspeedCollision = new ();
    public static UnityEvent OnFlyingForLongTime = new ();
    //only invoke very high and low speed events when it happens the first time
    public static UnityEvent OnVeryHighSpeed = new ();
    public static UnityEvent OnVeryLowSpeed = new ();
    
    private Rigidbody _rigidbody;
    //the time the penguin is not touchign a ground or structure
    private float _timeFlying = 0f;
    private bool _veryHighSpeedInvoked = false;
    //only fire if the penguin is in very low speed for 2 seconds
    private bool _veryLowSpeedInvoked = false;
    private float _timeInVeryLowSpeed = 0f;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (_rigidbody == null) return;
        var velocity = _rigidbody.velocity;
        if (velocity.magnitude >= SpeedController.Instance.maxSpeed-1f)
        {
            if (!_veryHighSpeedInvoked)
            {
                OnVeryHighSpeed.Invoke();
                _veryHighSpeedInvoked = true;
            }
        }
        else
        {
            _veryHighSpeedInvoked = false;
        }
        
        if (velocity.magnitude <= SpeedController.Instance.minSpeed+1f)
        {
            _timeInVeryLowSpeed += Time.fixedDeltaTime;
            if (!_veryLowSpeedInvoked && _timeInVeryLowSpeed >= 1f)
            {
                OnVeryLowSpeed.Invoke();
                _veryLowSpeedInvoked = true;
            }
        }
        else
        {
            _timeInVeryLowSpeed = 0f;
            _veryLowSpeedInvoked = false;
        }
        
        //check if the penguin is not touching a ground or structure
        if (Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Ground", "Structure")).Length == 0)
        {
            _timeFlying += Time.fixedDeltaTime;
            if (_timeFlying >= 2f)
            {
                OnFlyingForLongTime.Invoke();
            }
        }
        else
        {
            _timeFlying = 0f;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude >= 10f && other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            OnHighspeedCollision.Invoke();
        }
    }
}
