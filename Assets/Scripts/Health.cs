using System;
using UnityEngine;

[RequireComponent(typeof(ActionScheduler))]
public class Health : MonoBehaviour, IAction
{
    [SerializeField] private float _maxValue;
    [SerializeField] private Animator _animator;

    private ActionScheduler _actionScheduler;

    public bool IsDied { get; private set; }
    public float Value { get; private set; }

    public event Action Died;
    public event Action<float> Updated;

    private void Start()
    {
        _actionScheduler = GetComponent<ActionScheduler>();
        Value = _maxValue;
        Updated?.Invoke(Value);
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (IsDied)
            return;

        _actionScheduler.StartAction(this);
        Value = Mathf.Max(0, Value - damage);
        _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Hit);

        if (Value == 0)
            Die();

        Updated?.Invoke(Value);
    }

    public void Heal(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = Mathf.Min(_maxValue, Value + value);
        Updated?.Invoke(Value);
    }

    public void Cancel()
    {
        _animator.ResetTrigger(AnimatorCharacterManager.Instance.Params.Hit);
    }

    private void Die()
    {
        if (IsDied)
            return;

        IsDied = true;
        _animator.SetBool(AnimatorCharacterManager.Instance.Params.IsDie, true);
        _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Die);
        Died?.Invoke();
    }
}
