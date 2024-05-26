using UnityEngine;
using UnityEngine.UI;

public class ButtonDamage : MonoBehaviour, IDamageDealer
{
    [SerializeField] private Button _button;
    [SerializeField] private float _damage;
    [SerializeField] private MonoBehaviour _damageableMonoBehaviour;

    private IDamageable _damageable;

    public float Damage => _damage;

    private void OnValidate()
    {
        if (_damageableMonoBehaviour == null || _damageableMonoBehaviour is IDamageable)
            return;

        Debug.LogWarning($"{nameof(_damageableMonoBehaviour)} is not {nameof(IDamageable)}");
        _damageableMonoBehaviour = null;
    }

    private void Awake()
    {
        _damageable = (IDamageable)_damageableMonoBehaviour;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Attack);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Attack);
    }

    private void Attack()
    {
        _damageable.TakeDamage(this);
    }
}
