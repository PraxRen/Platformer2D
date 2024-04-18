using System;
using UnityEngine;

public class HendlerEventTakeDamageTransition : Transition
{
    private Fighter _fighter;

    public override void Activate()
    {
        _fighter.TookDamage += OnTookDamage;
    }

    protected override void RunActionBeforeInitialize(AIEnemyController aiController, State currentState)
    {
        if (aiController.TryGetComponent(out _fighter) == false)
        {
            throw new InvalidOperationException($"��� ������������� �������� \"{GetType().Name}\" ��������� ��������� \"{nameof(Fighter)}\"!");
        }
    }

    protected override void RunActionBeforeDeactivate()
    {
        _fighter.TookDamage -= OnTookDamage;
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
