using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    [SerializeField] private Animator _animator;

    public bool IsDied { get; private set; }
    public float Value { get; private set; }

    public event Action Died;

    private void Start()
    {
        Value = _maxValue;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (IsDied)
            return;

        Value = Mathf.Max(0, Value - damage);
        _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Hit);

        if (Value == 0)
            Die();
    }

    public void Heal(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = Mathf.Min(_maxValue, Value + value);
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
