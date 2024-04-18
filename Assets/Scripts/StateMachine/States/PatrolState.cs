using System;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private PatrolPath _defaultPatrolPath;
    [SerializeField] private float _offsetToWaypointPath;

    private Movement _movement;
    private Vector3 _waypointPosition;
    private int _currentIndexPath;

    protected override void RunActionBeforeInitialize(AIEnemyController aiController)
    {
        if (aiController.TryGetComponent(out _movement) == false)
        {
            throw new InvalidOperationException($"Для работы состояния \"{GetType().Name}\" необходим компонент \"{nameof(Movement)}\"!");
        }
    }

    protected override void RunActionBeforeEnter()
    {
        _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
    }

    protected override void Work()
    {
        if (Vector2.Distance(_waypointPosition, AIController.transform.position) < _offsetToWaypointPath)
        {
            _defaultPatrolPath.SetNextIndex(ref _currentIndexPath);
            _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
        }

        Vector2 direction = (_waypointPosition - AIController.transform.position).normalized;
        _movement.CancelRun();
        _movement.Move(direction);
    }
}
