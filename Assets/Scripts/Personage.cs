using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Personage : MonoBehaviour
{
    [SerializeField] protected float _health;
    [SerializeField] protected float _damage;

    protected Animator _animator;
    protected Personage _opponent;

    public bool IsDead { get; protected set; }

    public virtual void Hit(float damage)
    {
        if (IsDead == true)
            return;

        _animator.SetTrigger("Hit");
        _health -= damage;

        if (_health <= 0)
            Die();
    }

    public virtual void Attack()
    {
        _opponent?.Hit(_damage);
    }

    protected virtual void Die()
    {
        IsDead = true;
        _animator.SetTrigger("Death");
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
