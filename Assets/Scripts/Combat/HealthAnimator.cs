using UnityEngine;

public class HealthAnimator : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _health.OnTookDamage += OnTookDamage;
        _health.CancelTookDamage += CancelTookDamage;
        _health.OnDied += OnDied;
    }

    private void OnDisable()
    {
        _health.OnTookDamage -= OnTookDamage;
        _health.OnDied -= OnDied;
    }

    private void OnDied()
    {
        _animator.SetBool(CharacterAnimatorData.Params.IsDie, true);
        _animator.SetTrigger(CharacterAnimatorData.Params.Die);
    }

    private void OnTookDamage(IDamageDealer damageDealer) => _animator.SetTrigger(CharacterAnimatorData.Params.Hit);

    private void CancelTookDamage() => _animator.ResetTrigger(CharacterAnimatorData.Params.Hit);
}