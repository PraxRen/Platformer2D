using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IAction, IAttribute
{
    [SerializeField] private float _maxValue;
    [SerializeField] private ActionScheduler _actionScheduler;

    public event Action Died;
    public event Action<IDamageDealer> TookDamage;
    public event Action CancelTookDamage;
    public event Action ValueChanged;

    public bool IsDied { get; private set; }
    public float Value { get; private set; }
    public float MaxValue => _maxValue;
    public Vector3 Position => transform.position;

    private void Start()
    {
        UpdateValue(_maxValue);
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        if (damageDealer == null)
            throw new ArgumentNullException(nameof(damageDealer));

        if (damageDealer.Damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damageDealer.Damage));

        if (IsDied)
            return;

        _actionScheduler.StartAction(this);
        UpdateValue(Value - damageDealer.Damage);
        TookDamage?.Invoke(damageDealer);

        if (Value == 0)
            Die();
    }

    public float CalculateDamage(IDamageDealer damageDealer)
    {
        if (damageDealer == null)
            throw new ArgumentNullException(nameof(damageDealer));

        if (damageDealer.Damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damageDealer.Damage));

        if (IsDied)
            return 0;

        float valueAfterDamage = Value - damageDealer.Damage;

        return valueAfterDamage > 0 ? damageDealer.Damage : Value;
    }

    public void Heal(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        UpdateValue(Value + value);
    }

    public void Cancel() => CancelTookDamage?.Invoke();

    private void UpdateValue(float value)
    {
        Value = Mathf.Clamp(value, 0, _maxValue);
        ValueChanged?.Invoke();
    }

    private void Die()
    {
        if (IsDied)
            return;

        IsDied = true;
        Died?.Invoke();
    }
}