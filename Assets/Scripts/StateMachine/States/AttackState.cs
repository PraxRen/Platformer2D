using System;
using System.Collections;
using UnityEngine;

public class AttackState : State, IDamageableConsumer
{
    [Range(0, 1)][SerializeField] private float _timeWaitMoveToTarget;

    private Fighter _fighter;
    private Mover _mover;
    private IDamageable _currentTarget;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobMoveToTatget;

    public IDamageable GetDamageable() => _currentTarget;

    public void SetDamageable(IDamageable damageable)
    {
        if (Status != StatusState.Initialized && Status != StatusState.Exited)
            return;

        _currentTarget = damageable;
    }

    protected override void RunActionBeforeInitialize(AIEnemy aiEnemy)
    {
        if (aiEnemy.TryGetComponent(out _fighter) == false)
        {
            throw new InvalidOperationException($"Для инициализации состояния \"{GetType().Name}\" необходим компонент \"{nameof(Fighter)}\"!");
        }

        if (aiEnemy.TryGetComponent(out _mover) == false)
        {
            throw new InvalidOperationException($"Для инициализации состояния \"{GetType().Name}\" необходим компонент \"{nameof(Mover)}\"!");
        }

        _waitForSeconds = new WaitForSeconds(_timeWaitMoveToTarget);
    }

    protected override void RunActionBeforeEnter()
    {
        if (_currentTarget == null)
        {
            throw new InvalidOperationException($"Для входа в состояние \"{GetType().Name}\" необходимо иметь значение для \"{nameof(_currentTarget)}\"!");
        }
    }

    protected override void RunActionAfterEnter()
    {
        _jobMoveToTatget = StartCoroutine(MoveToTatget());
    }

    protected override void RunActionBeforeExit()
    {
        StopCoroutine(_jobMoveToTatget);
        _currentTarget = null;
    }

    protected override void Work()
    {
        if (_fighter.CanAttack() == false)
            return;

        if (_fighter.CanHitTarget(_currentTarget) == false)
            return;

        _fighter.Attack();
    }

    private IEnumerator MoveToTatget()
    {
        while (Status == StatusState.Entered)
        {
            Vector2 direction = (_currentTarget.Position - AIEnemy.transform.position).normalized;
            _mover.LookAtDirection(direction);

            if (Vector2.Distance(_currentTarget.Position, AIEnemy.transform.position) >= _fighter.DistanceDamage)
            {
                _mover.Run();
                _mover.Move(direction);
            }
            else
            {
                _mover.Stop();
            }

            yield return _waitForSeconds;
        }
    }
}