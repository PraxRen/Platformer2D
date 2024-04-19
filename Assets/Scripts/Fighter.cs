using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D), typeof(Health), typeof(ActionScheduler))]
public class Fighter : MonoBehaviour, IDamageable, IDamageDealer, IListenerAnimationEvent, IAction
{
    [SerializeField] private float _attackTimeout;
    [SerializeField] private float _damage;
    [SerializeField] private float _radiusDamage;
    [SerializeField] private float _distanceDamage;
    [SerializeField] private LayerMask _layerDamageble;
    [SerializeField] private Animator _animator;
    [SerializeField] private HandlerAnimationEvent _handlerAnimationEvent;

    private ActionScheduler _actionScheduler;
    private CapsuleCollider2D _collider;
    private Health _health;
    private float _currentAttackTimeout;
    private Coroutine _jobRunTimerAttack;
    private IDamageable _target;
    private bool isAttack;

    public event Action<IDamageDealer> TookDamage;

    public float Damage => _damage;
    public float DistanceDamage => _distanceDamage;
    public Vector3 Position => transform.position;

    private void OnDisable()
    {
        CancelTimerAttack();
        _handlerAnimationEvent.RemoveAction(TypeAnimationEvent.StartAnimationtAttack, this, HandleAttack);
        isAttack = false;
    }

    private void Start()
    {
        _actionScheduler = GetComponent<ActionScheduler>();
        _collider = GetComponent<CapsuleCollider2D>();
        _health = GetComponent<Health>();
        _handlerAnimationEvent.AddAction(TypeAnimationEvent.StartAnimationtAttack, this, HandleAttack);
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        _health.TakeDamage(damageDealer.Damage);
        TookDamage?.Invoke(damageDealer);
    }

    public bool CanAttack()
    {
        if (isAttack)
            return false;

        if (_health.IsDied)
            return false;

        if (_currentAttackTimeout > 0f)
            return false;

        return true;
    }

    public void Attack()
    {
        _actionScheduler.StartAction(this);
        isAttack = true;
        CancelTimerAttack();
        _jobRunTimerAttack = StartCoroutine(RunTimerAttack());
        _animator.SetTrigger(CharacterAnimatorData.Params.Attack);
    }

    public void Attack(Fighter fighter)
    {
        _target = fighter;
        Attack();
    }

    public bool CanHitTarget(IDamageable target)
    {
        IDamageable damageable = FindTarget();

        if (damageable == null) 
            return false;

        if (damageable != target)
            return false;

        return true;
    }

    public bool HasTargetsInRadius(float radius, out IEnumerable<IDamageable> damageables)
    {
        damageables = null;
        bool result = false;
        float height = _collider.size.y / 2;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + height), radius, _layerDamageble);

        if (colliders.Length < 0)
            return result;

        List<IDamageable> foundDamageables = new List<IDamageable>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                foundDamageables.Add(damageable);
                result = true;
            }
        }

        damageables = foundDamageables;
        return result;
    }

    public void Cancel()
    {
        CancelAttack();
    }

    private void HandleAttack()
    {
        if (_target == null)
        {
            IDamageable target = FindTarget();

            if (target == null)
            {
                CancelAttack();
                return;
            }

            _target = target;
        }

        AttackTarget();
    }

    private void AttackTarget()
    {
        _target.TakeDamage(this);
        CancelAttack();
    }

    private void CancelAttack()
    {
        _animator.ResetTrigger(CharacterAnimatorData.Params.Attack);
        _target = null;
        isAttack = false;
    }

    private IDamageable FindTarget()
    {
        RaycastHit2D hit = GetHit();

        if (hit.collider == null)
            return null;

        if (hit.transform.TryGetComponent(out IDamageable damageable) == false)
            return null;

        return damageable;
    }

    private RaycastHit2D GetHit()
    {
        float height = _collider.size.y / 2;
        Vector3 offset = new Vector2(transform.localScale.normalized.x * (_radiusDamage / 2), height);
        Vector2 direction = new Vector2(transform.localScale.normalized.x, 0);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + offset, _radiusDamage, direction, _distanceDamage, _layerDamageble);
        return hit;
    }

    private IEnumerator RunTimerAttack()
    {
        _currentAttackTimeout = _attackTimeout;

        while (_currentAttackTimeout > 0f)
        {
            _currentAttackTimeout -= Time.deltaTime;
            yield return null;
        }

        _currentAttackTimeout = 0f;
        _jobRunTimerAttack = null;
    }

    private void CancelTimerAttack()
    {
        if (_jobRunTimerAttack == null)
            return;

        StopCoroutine(_jobRunTimerAttack);
        _jobRunTimerAttack = null;
        _currentAttackTimeout = 0f;
    }

    private void DebugDrawCircle(Vector2 center, float radius, Color color)
    {
        int segments = 36;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            float x = center.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = center.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Vector2 startPoint = new Vector2(x, y);

            angle += angleStep;
            x = center.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            y = center.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Vector2 endPoint = new Vector2(x, y);

            Debug.DrawLine(startPoint, endPoint, color);
        }
    }
}