using System;
using UnityEngine;

public class HendlerEventTakeDamageTransition : Transition
{
    private Fighter _fighter;

    public override void Activate()
    {
        _fighter.OnTookDamage += OnTookDamage;
    }

    protected override void RunActionBeforeInitialize(AIEnemy aiEnemy, State currentState)
    {
        if (aiEnemy.TryGetComponent(out _fighter) == false)
        {
            throw new InvalidOperationException($"Для инициализации перехода \"{GetType().Name}\" необходим компонент \"{nameof(Fighter)}\"!");
        }
    }

    protected override void RunActionBeforeDeactivate()
    {
        _fighter.OnTookDamage -= OnTookDamage;
    }

    private void OnTookDamage(IDamageDealer damageDealer)
    {
        if (damageDealer is not IDamageable damageable)
            return;

        if (TargetState is IDamageableConsumer damageableConsumer)
            damageableConsumer.SetDamageable(damageable);

        NeedTransit = true;
    }
}
