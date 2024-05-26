using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IAction, IAttribute
{
    [SerializeField] private float _maxValue;
    [SerializeField] private ActionScheduler _actionScheduler;

    public event Action OnDied;
    public event Action<IDamageDealer> OnTookDamage;
    public event Action CancelTookDamage;
    public event Action OnValueChanged;

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
        OnTookDamage?.Invoke(damageDealer);

        if (Value == 0)
            Die();
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
        OnValueChanged?.Invoke();
    }

    private void Die()
    {
        if (IsDied)
            return;

        IsDied = true;
        OnDied?.Invoke();
    }
}