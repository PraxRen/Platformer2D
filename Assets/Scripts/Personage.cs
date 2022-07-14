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

    private const string AnimationParameterAttack = "Attack";
    private const string AnimationParameterHit = "Hit";
    private const string AnimationParameterJump = "Jump";
    private const string AnimationParameterMove = "Speed";
    private const string AnimationParameterDie = "Death";

    protected Animator _animator;
    protected Personage _opponent;

    public bool IsDead { get; protected set; }

    public virtual void Hit(float damage)
    {
        if (IsDead == true)
            return;

        _animator.SetTrigger(AnimationParameterHit);
        _health -= damage;

        if (_health <= 0)
            Die();
    }

    public virtual void Attack()
    {
        _opponent?.Hit(_damage);
        _animator.SetTrigger(AnimationParameterAttack);
    }

    public virtual void Jump()
    {
        _animator.SetTrigger(AnimationParameterJump);
    }

    public virtual void Move(float speed)
    {
        if (_animator == null)
            return;

        _animator.SetFloat("Speed", Math.Abs(speed));
    }

    protected virtual void Die()
    {
        IsDead = true;
        _animator.SetTrigger(AnimationParameterDie);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
