using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SearcherDamageableTransition : Transition
{
    [SerializeField] private float _distance;
    [Range(0, 5)][SerializeField] private float _timeWaitFind;

    private Fighter _fighter;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobFind;

    public override void Activate()
    {
        _jobFind = StartCoroutine(Find());
    }

    protected override void RunActionBeforeInitialize(AIEnemyController aiController, State currentState)
    {
        if (aiController.TryGetComponent(out _fighter) == false)
        {
            throw new InvalidOperationException($"Для инициализации перехода \"{GetType().Name}\" необходим компонент \"{nameof(Fighter)}\"!");
        }

        _waitForSeconds = new WaitForSeconds(_timeWaitFind);
    }

    protected override void RunActionBeforeDeactivate()
    {
        if (_jobFind != null)
        {
            StopCoroutine(_jobFind);
        }
    }

    private IEnumerator Find()
    {
        while (NeedTransit == false)
        {
            if (_fighter.HasTargetsInRadius(_distance, out IEnumerable<IDamageable> damageables))
            {
                IDamageable damageable = damageables.OrderBy(damageable => Vector2.Distance(damageable.Position, _fighter.transform.position)).First();

                if (TargetState is IDamageableConsumer damageableConsumer)
                    damageableConsumer.SetDamageable(damageable);

                NeedTransit = true;
            }

            yield return _waitForSeconds;
        }

        _jobFind = null;
    }
}