using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D), typeof(ActionScheduler))]
public class Fighter : MonoBehaviour, IDamageable, IDamageDealer, IListenerAnimationEvent, IAction
{
    [SerializeField] private float _attackTimeout;
    [SerializeField] private float _damage;
    [SerializeField] private float _radiusDamage;
    [SerializeField] private float _distanceDamage;
    [SerializeField] private LayerMask _layerDamageble;
    [SerializeField] private Animator _animator;
    [SerializeField] private HandlerAnimationEvent _handlerAnimationEvent;
    [SerializeField] private MonoBehaviour _damageableMonoBehaviour;

    private IDamageable _damageable;
    private IDamageable _target;
    private ActionScheduler _actionScheduler;
    private CapsuleCollider2D _collider;
    private float _currentAttackTimeout;
    private Coroutine _jobRunTimerAttack;
    private bool isAttack;

    public event Action<IDamageDealer> OnTookDamage;

    public float Damage => _damage;
    public float DistanceDamage => _distanceDamage;
    public Vector3 Position => transform.position;
    public bool IsDied => _damageable.IsDied;

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
        _actionScheduler = GetComponent<ActionScheduler>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void OnDisable()
    {
        CancelTimerAttack();
        _handlerAnimationEvent.RemoveAction(TypeAnimationEvent.StartAnimationtAttack, this, HandleAttack);
        isAttack = false;
    }

    private void Start()
    {
        _handlerAnimationEvent.AddAction(TypeAnimationEvent.StartAnimationtAttack, this, HandleAttack);
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        _damageable.TakeDamage(damageDealer);
        OnTookDamage?.Invoke(damageDealer);
    }

    public bool CanAttack()
    {
        if (isAttack)
            return false;

        if (_damageable.IsDied)
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