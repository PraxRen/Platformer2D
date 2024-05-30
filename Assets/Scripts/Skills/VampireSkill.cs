using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVampireSkill", menuName = "Skill/VampireSkill", order = 0)]
public class VampireSkill : Skill, IDamageDealer
{
    private const string IdCoroutine = "VampireSkill";

    [SerializeField] private float _radius;
    [SerializeField] private float _power;

    public float Damage => _power;

    public override bool CanActivate(Player player)
    {
        if (player is null)
            throw new ArgumentNullException(nameof(player));

        Health health =  player.GetComponentInChildren<Health>();
        
        if (health == null)
            return false;

        if (health.IsDied)
            return false;

        if (player.TryGetComponent(out Fighter fighter) == false)
            return false;

        if (player.TryGetComponent(out CoroutineRunner coroutineRunner) == false)
            return false;

        return true;
    }

    public override void Activate(Player player)
    {
        if (player is null)
            throw new ArgumentNullException(nameof(player));

        CoroutineRunner coroutineRunner = player.GetComponent<CoroutineRunner>();
        Health health = player.GetComponentInChildren<Health>();
        Fighter fighter = player.GetComponent<Fighter>();
        coroutineRunner.Create(player.Id + IdCoroutine, () => Run(fighter, health));
    }

    public override void Deactivate(Player player)
    {
        if (player is null)
            throw new ArgumentNullException(nameof(player));

        CoroutineRunner coroutineRunner = player.GetComponent<CoroutineRunner>();
        coroutineRunner.Destroy(player.Id + IdCoroutine);
    }

    private void Run(Fighter fighter, Health health)
    {
        if (fighter.HasTargetInRadius(_radius, out IDamageable damageable) == false)
            return;

        damageable.TakeDamage(this);
        health.Heal(_power);
    }
}