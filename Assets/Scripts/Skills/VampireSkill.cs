using System;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVampireSkill", menuName = "Skill/VampireSkill", order = 0)]
public class VampireSkill : Skill, IDamageDealer
{
    private const string IdCoroutine = "VampireSkill";
    private const float TimeWaitForSeconds = 1f;

    [SerializeField] private float _radius;
    [SerializeField] private float _power;

    private StringBuilder _stringBuilder = new StringBuilder();

    public float Damage => _power;

    public override bool TryValidate(Character character, out string warning)
    {
        bool result = true;

        if (character.TryGetComponent(out HealthReferenceHelper healthReferenceHelper) == false && healthReferenceHelper.Health == null)
        {
            _stringBuilder.AppendLine($"У {nameof(character)} отсутстует компонент {nameof(Health)}");
            result = false;
        }

        if (character.TryGetComponent(out Fighter fighter) == false)
        {
            _stringBuilder.AppendLine($"У {nameof(character)} отсутстует компонент {nameof(Fighter)}");
            result = false;
        }

        if (character.TryGetComponent(out CharacterCoroutineRunner coroutineRunner) == false)
        {
            _stringBuilder.AppendLine($"У {nameof(character)} отсутстует компонент {nameof(CharacterCoroutineRunner)}");
            result = false;
        }

        if (character.TryGetComponent(out CapsuleCollider2D collider) == false)
        {
            _stringBuilder.AppendLine($"У {nameof(character)} отсутстует компонент {nameof(CapsuleCollider2D)}");
            result = false;
        }

        warning = _stringBuilder.ToString();
        _stringBuilder.Clear();
        return result;
    }

    public override bool TryActivate(Character character)
    {
        if (character is null)
            throw new ArgumentNullException(nameof(character));

        if (character.TryGetComponent(out HealthReferenceHelper healthReferenceHelper) == false && healthReferenceHelper.Health == null)
            return false;

        if (healthReferenceHelper.Health.IsDied)
            return false;

        if (character.TryGetComponent(out Fighter fighter) == false)
            return false;

        if (character.TryGetComponent(out CharacterCoroutineRunner coroutineRunner) == false)
            return false;

        if (character.TryGetComponent(out CapsuleCollider2D collider) == false)
            return false;

        if (character.TryGetComponent(out ActivatorGraphics activatorGraphics) == false)
            return false;

        float heightOffset = collider.size.y / 2;
        Vector2 offset = new Vector2(0, heightOffset);
        activatorGraphics.Activate(TypeGraphics.VampireSkill, new Vector2(fighter.transform.position.x + offset.x, fighter.transform.position.y + offset.y), new Vector2(_radius, _radius));
        coroutineRunner.Create(character.Id + IdCoroutine, () => Run(fighter, healthReferenceHelper.Health, offset), TimeWaitForSeconds);
        return true;
    }

    public override bool TryDeactivate(Character character)
    {
        if (character is null)
            throw new ArgumentNullException(nameof(character));

        if (character.TryGetComponent(out CharacterCoroutineRunner coroutineRunner) == false)
            return false;

        if (character.TryGetComponent(out ActivatorGraphics activatorGraphics) == false)
            return false;

        activatorGraphics.Deactivate(TypeGraphics.VampireSkill);
        coroutineRunner.Destroy(character.Id + IdCoroutine);
        return true;
    }

    private void Run(Fighter fighter, Health health, Vector2 offset)
    {
        Vector2 startPosition = new Vector2(fighter.transform.position.x + offset.x, fighter.transform.position.y + offset.y);

        if (fighter.HasTargetInRadius(_radius, startPosition, out IDamageable damageable) == false)
            return;

        if (damageable.IsDied)
            return;

        float healPoint = damageable.CalculateDamage(this);
        damageable.TakeDamage(this);
        health.Heal(healPoint);
    }
}