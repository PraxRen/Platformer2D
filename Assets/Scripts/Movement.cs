using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _powerJump;
    private static float _ranSpeedMulti = 2;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-_speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(_speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, _powerJump * Time.deltaTime, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed / _ranSpeedMulti;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _ranSpeedMulti;
        }
    }
}