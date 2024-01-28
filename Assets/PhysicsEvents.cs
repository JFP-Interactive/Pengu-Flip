using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsEvents : MonoBehaviour
{
    public UnityEvent OnHighspeedCollision = new ();
    public UnityEvent OnFlyingForLongTime = new ();
    //only invoke very high and low speed events when it happens the first time
    public UnityEvent OnVeryHighSpeed = new ();
    public UnityEvent OnVeryLowSpeed = new ();
    
    private Rigidbody _rigidbody;
    //the time the penguin is not touchign a ground or structure
    private float _timeFlying = 0f;
    private bool _veryHighSpeedInvoked = false;
    //only fire if the penguin is in very low speed for 2 seconds
    private bool _veryLowSpeedInvoked = false;
    private bool _flyingForLongTimeInvoked = false;
    private float _timeInVeryLowSpeed = 0f;
    
    public static PhysicsEvents Instance { get; private set; }
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Instance = this;
    }
    
    private void FixedUpdate()
    {
        if (_rigidbody == null) return;
        var velocity = _rigidbody.velocity;
        if (velocity.magnitude >= SpeedController.Instance.maxSpeed-1f)
        {
            if (!_veryHighSpeedInvoked)
            {
                _veryHighSpeedInvoked = true;
                OnVeryHighSpeed.Invoke();
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
                _veryLowSpeedInvoked = true;
                OnVeryLowSpeed.Invoke();
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
                if (!_flyingForLongTimeInvoked)
                {
                    OnFlyingForLongTime.Invoke();
                    _flyingForLongTimeInvoked = true;
                }
            }
        }
        else
        {
            _flyingForLongTimeInvoked = false;
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
