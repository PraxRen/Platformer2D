using UnityEngine;

public class Thorn : MonoBehaviour, IDamageDealer
{
    [SerializeField] private float _damage;

    public float Damage => _damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(this);
        }
    }
}