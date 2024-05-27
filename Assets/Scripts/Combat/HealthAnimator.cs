using UnityEngine;

public class HealthAnimator : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _health.TookDamage += OnTookDamage;
        _health.CancelTookDamage += CancelTookDamage;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.TookDamage -= OnTookDamage;
        _health.Died -= OnDied;
    }

    private void OnDied()
    {
        _animator.SetBool(CharacterAnimatorData.Params.IsDie, true);
        _animator.SetTrigger(CharacterAnimatorData.Params.Die);
    }

    private void OnTookDamage(IDamageDealer damageDealer) => _animator.SetTrigger(CharacterAnimatorData.Params.Hit);

    private void CancelTookDamage() => _animator.ResetTrigger(CharacterAnimatorData.Params.Hit);
}