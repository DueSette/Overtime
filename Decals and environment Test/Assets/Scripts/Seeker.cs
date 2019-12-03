using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] float _maxSpeed;
    [SerializeField] GameObject _quarry;
    [SerializeField] float _arrivalRadius;
    [SerializeField] float _predictionMargin;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       //_rb.AddForce(SteeringMethods.Arrival2D(transform, _quarry.transform, _maxSpeed, _arrivalRadius) ?? Vector3.zero, ForceMode2D.Force);
        _rb.AddForce(SteeringMethods.Pursue(transform, _quarry.transform, _maxSpeed));

        //transform.LookAt(_quarry.transform.position);
    }
}