using System;
using UnityEngine;

[RequireComponent(typeof(ActionScheduler))]
public class Health : MonoBehaviour, IAction, IAttribute
{
    [SerializeField] private float _maxValue;
    [SerializeField] private Animator _animator;

    private ActionScheduler _actionScheduler;

    public event Action Died;
    public event Action OnValueChanged;

    public bool IsDied { get; private set; }
    public float Value { get; private set; }
    public float MaxValue => _maxValue;

    private void Start()
    {
        _actionScheduler = GetComponent<ActionScheduler>();
        Value = _maxValue;
        OnValueChanged?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (IsDied)
            return;

        _actionScheduler.StartAction(this);
        Value = Mathf.Max(0, Value - damage);
        _animator.SetTrigger(CharacterAnimatorData.Params.Hit);

        if (Value == 0)
            Die();

        OnValueChanged?.Invoke();
    }

    public void Heal(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = Mathf.Min(_maxValue, Value + value);
        OnValueChanged?.Invoke();
    }

    public void Cancel()
    {
        _animator.ResetTrigger(CharacterAnimatorData.Params.Hit);
    }

    private void Die()
    {
        if (IsDied)
            return;

        IsDied = true;
        _animator.SetBool(CharacterAnimatorData.Params.IsDie, true);
        _animator.SetTrigger(CharacterAnimatorData.Params.Die);
        Died?.Invoke();
    }
}
