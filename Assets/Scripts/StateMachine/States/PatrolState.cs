using System;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private PatrolPath _defaultPatrolPath;
    [SerializeField] private float _offsetToWaypointPath;

    private Mover _mover;
    private Vector3 _waypointPosition;
    private int _currentIndexPath;

    protected override void RunActionBeforeInitialize(AIEnemy aiEnemy)
    {
        if (aiEnemy.TryGetComponent(out _mover) == false)
        {
            throw new InvalidOperationException($"Для работы состояния \"{GetType().Name}\" необходим компонент \"{nameof(Mover)}\"!");
        }
    }

    protected override void RunActionBeforeEnter()
    {
        _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
    }

    protected override void Work()
    {
        if (Vector2.Distance(_waypointPosition, AIEnemy.transform.position) < _offsetToWaypointPath)
        {
            _defaultPatrolPath.SetNextIndex(ref _currentIndexPath);
            _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
        }

        Vector2 direction = (_waypointPosition - AIEnemy.transform.position).normalized;
        _mover.CancelRun();
        _mover.Move(direction);
    }
}
