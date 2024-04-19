using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class LossDamageableTransition : Transition
{
    [SerializeField] private float _distance;
    [Range(0, 5)][SerializeField] private float _timeWaitFind;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobFind;
    private IDamageableConsumer _damageableConsumer;

    public override void Activate()
    {
        IDamageable damageable = _damageableConsumer.GetDamageable();

        if (damageable == null)
        {
            throw new ArgumentNullException($"Для активации перехода \"{GetType().Name}\" необходимо установить значение \"{nameof(damageable)}\"");
        }

        _jobFind = StartCoroutine(Find());
    }

    protected override void RunActionBeforeInitialize(AIEnemy aiEnemy, State currentState)
    {
        _damageableConsumer = currentState as IDamageableConsumer;

        if (_damageableConsumer == null)
        {
            throw new InvalidOperationException($"Для инициализации перехода \"{GetType().Name}\" необходимо чтобы текущее состояние реализовывало интерфейс \"{nameof(IDamageableConsumer)}\"!");
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
            IDamageable damageable = _damageableConsumer.GetDamageable();

            if (Vector2.Distance(damageable.Position, transform.position) >= _distance)
                NeedTransit = true;

            yield return _waitForSeconds;
        }

        _jobFind = null;
    }
}
