using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    
    private float _speedCurrent = 0;
    private static float _ranSpeedMulti = 2;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _speedCurrent = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _ranSpeedMulti;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= _ranSpeedMulti;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _speedCurrent = -_speed;
            
            if(transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _speedCurrent = _speed;

            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        transform.Translate(_speedCurrent * Time.deltaTime, 0, 0);
        _animator.SetFloat("Speed", Math.Abs(_speedCurrent));

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, _jumpForce * Time.deltaTime, 0);
            _animator.SetTrigger("Jump");
        }
    }
}