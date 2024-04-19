using System;
using UnityEngine;

[RequireComponent(typeof(ActionScheduler))]
public class Health : MonoBehaviour, IAction
{
    [SerializeField] private float _maxValue;
    [SerializeField] private Animator _animator;

    private ActionScheduler _actionScheduler;

    public event Action Died;
    public event Action<float> ValueChenged;

    public bool IsDied { get; private set; }
    public float Value { get; private set; }

    private void Start()
    {
        _actionScheduler = GetComponent<ActionScheduler>();
        Value = _maxValue;
        ValueChenged?.Invoke(Value);
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

        ValueChenged?.Invoke(Value);
    }

    public void Heal(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = Mathf.Min(_maxValue, Value + value);
        ValueChenged?.Invoke(Value);
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
