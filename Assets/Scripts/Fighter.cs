using System;
using System.Collections;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Health))]
public class Fighter : MonoBehaviour, IListenerAnimationEvent
{
    [SerializeField] private float _attackTimeout;
    [SerializeField] private float _damage;
    [SerializeField] private float _radiusDamage;
    [SerializeField] private float _distanceDamage;
    [SerializeField] private LayerMask _layerDamageble;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationEventController _animationEventController;

    private CapsuleCollider2D _collider;
    private Health _health;
    private float _currentAttackTimeout;
    private Coroutine _jobRunTimerAttack;

    public float Damage => _damage;

    public event Action TookDamage;

    private void OnEnable()
    {
        _animationEventController.AddAction(TypeAnimationEvent.StartAnimationtAttack, this, AttemptAttackTarget);
    }

    private void OnDisable()
    {
        CancelTimerAttack();
        _animationEventController.RemoveAction(TypeAnimationEvent.StartAnimationtAttack, this, AttemptAttackTarget);
    }

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _health = GetComponent<Health>();
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
        TookDamage?.Invoke();
    }

    public bool CanAttack()
    {
        if (_health.IsDied)
            return false;

        if (_currentAttackTimeout > 0f)
            return false;

        return true;
    }

    public void Attack()
    {
        CancelTimerAttack();
        _jobRunTimerAttack = StartCoroutine(RunTimerAttack());
        _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Attack);
    }

    public bool CanHitTarget(float distance) 
    {
        RaycastHit2D[] hits = GetHist(distance);

        if (hits.Length == 0)
            return false;

        hits = hits.OrderBy(hit => Vector3.Distance(transform.position, hit.transform.position)).ToArray();
        return hits[0].transform.TryGetComponent(out Fighter fighter);
    }

    private void AttemptAttackTarget()
    {
        RaycastHit2D[] hits = GetHist(_distanceDamage);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.TryGetComponent(out Fighter fighter))
                fighter.TakeDamage(Damage);
        }
    }

    private RaycastHit2D[] GetHist(float distance)
    {
        float height = _collider.size.y / 2;
        Vector3 offset = new Vector2(transform.localScale.normalized.x * (_radiusDamage / 2), height);
        Vector2 direction = new Vector2(transform.localScale.normalized.x, 0);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + offset, _radiusDamage, direction, distance, _layerDamageble);
        return hits;
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