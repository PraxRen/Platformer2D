using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(DirectionVisual))]
public class WayPointMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _waySeconds;

    private int _currentPoint;
    private Animator _animator;
    private DirectionVisual _directionVisual;
    private Coroutine moveGameObjectJob;
    private Transform[] _savePoints;

    public void Pursuit(Transform target)
    {
        _points = new Transform[] { target };
    }

    public void Patrul()
    {
        _points = _savePoints;
    }

    private void OnEnable()
    {
        _savePoints = _points;
        _animator = GetComponent<Animator>();
        _directionVisual = GetComponent<DirectionVisual>();
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
            _directionVisual.ChangeDirection(direction);
            _animator.SetFloat("Speed", _speed);
            transform.position = newPosition;

            if (transform.position == _points[_currentPoint].position)
            {
                _currentPoint++;

                if (_currentPoint >= _points.Length)
                {
                    _currentPoint = 0;
                }

                _animator.SetFloat("Speed", 0);
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


