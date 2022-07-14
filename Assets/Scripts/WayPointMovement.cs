using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OrientationSpace), typeof(Personage))]
public class WayPointMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _waySeconds;

    private int _currentPoint;
    private Personage _personage;
    private OrientationSpace _orientationSpace;
    private Coroutine moveGameObjectJob;
    private Transform[] _savePoints;

    public void Pursuit(Transform target)
    {
        _points = new Transform[] { target };
        _currentPoint = 0;
    }

    public void Patrul()
    {
        _points = _savePoints;
    }

    private void OnEnable()
    {
        _savePoints = _points;
        _personage = GetComponent<Personage>();
        _orientationSpace = GetComponent<OrientationSpace>();
        moveGameObjectJob = StartCoroutine(MoveGameObject());
    }

    private void OnDisable()
    {
        StopCoroutine(moveGameObjectJob);
    }

    private IEnumerator MoveGameObject()
    {
        WaitForSeconds waitForSeconds;

        while (true)
        {
            waitForSeconds = null;
            var newPosition = Vector3.MoveTowards(transform.position, _points[_currentPoint].position, _speed * Time.deltaTime);
            var direction = GetTargetDirection(newPosition.x);
            _orientationSpace.ChangeDirection(direction);
            _personage.Move(_speed);
            transform.position = newPosition;

            if (transform.position == _points[_currentPoint].position)
            {
                _currentPoint++;

                if (_currentPoint >= _points.Length)
                {
                    _currentPoint = 0;
                }

                _personage.Move(0);
                waitForSeconds = new WaitForSeconds(_waySeconds);
            }

            yield return waitForSeconds;
        }
    }

    private Direction GetTargetDirection(float newPositionX)
    {
        Direction direction = default;

        if (newPositionX < transform.position.x)
            direction = Direction.Left;
        else
            direction = Direction.Right;

        return direction;
    }
}


