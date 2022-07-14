using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Personage), typeof(OrientationSpace))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _powerJump;
    
    private static float _ranSpeedMulti = 2;
    private float _currentSpeed;
    private OrientationSpace _orientationSpace;
    private Personage _personage;

    private void Start()
    {
        _orientationSpace = GetComponent<OrientationSpace>();
        _personage = GetComponent<Personage>();
    }

    private void Update()
    {
        _currentSpeed = 0;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed / _ranSpeedMulti;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _ranSpeedMulti;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _currentSpeed = -_speed;
            _orientationSpace.ChangeDirection(Direction.Left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _currentSpeed = _speed;
            _orientationSpace.ChangeDirection(Direction.Right);
        }

        transform.Translate(_currentSpeed * Time.deltaTime, 0, 0);
        _personage.Move(_currentSpeed); //_currentSpeed

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, _powerJump * Time.deltaTime, 0);
            _personage.Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _personage.Attack();
        }
    }
}