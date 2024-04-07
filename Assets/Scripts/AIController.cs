using System.Data;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Fighter))]
[RequireComponent(typeof(Health))]
public class AIController : MonoBehaviour
{
    [SerializeField] private PatrolPath _defaultPatrolPath;
    [SerializeField] private float _offsetToWaypointPath;

    private const float MinDistanceForAttack = 2.5f;
    private const float MaxDistanceForAttack = 5f;
    private const float MultiDistanceForAggression = 2f;
    private const float MinDistanceForAggression = MinDistanceForAttack / MultiDistanceForAggression;

    private Movement _movement;
    private Fighter _fighter;
    private Health _health;
    private int _currentIndexPath;
    private Vector3 _waypointPosition;
    private Transform _playerTransform;
    private TypeState _typeState;
    private bool _isTookDamage;

    private void OnEnable()
    {
        if (_health != null)
            _health.Died += OnDied;

        if (_fighter != null)
            _fighter.TookDamage += OnTookDamage;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
        _fighter.TookDamage -= OnTookDamage;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _fighter = GetComponent<Fighter>();
        _health = GetComponent<Health>();
        _health.Died += OnDied;
        _fighter.TookDamage += OnTookDamage;
        _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
        _playerTransform = PlayerController.Instance.transform;
        SetState(TypeState.Patrol);
    }

    private void Update()
    {
        UpdateState();

        switch (_typeState)
        {
            case TypeState.Patrol:
                HandlePatrol();
                break;
            case TypeState.Attack:
                HandleAttack();
                break;
        }
    }

    private void HandlePatrol()
    {
        if (Vector2.Distance(_waypointPosition, transform.position) < _offsetToWaypointPath)
        {
            _defaultPatrolPath.SetNextIndex(ref _currentIndexPath);
            _waypointPosition = _defaultPatrolPath.GetWaypointPosition(_currentIndexPath);
        }

        Vector2 direction = (_waypointPosition - transform.position).normalized;
        _movement.CancelRun();
        _movement.Move(direction);
    }

    private void HandleAttack()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;

        if (Vector2.Distance(_playerTransform.position, transform.position) >= MinDistanceForAttack)
        {
            _movement.Run();
            _movement.Move(direction);
        }

        if (_fighter.CanAttack() == false)
            return;

        _movement.LookAtDirection(direction);
        _fighter.Attack();
    }

    private void UpdateState()
    {
        switch (_typeState)
        {
            case TypeState.Patrol:
                UpdateStatePatrol();
                break;
            case TypeState.Attack:
                UpdateStateAttack();
                break;
        }
    }

    private void UpdateStatePatrol()
    {
        if (_isTookDamage)
        {
            SetState(TypeState.Attack);
            _isTookDamage = false;
            return;
        }

        if (Vector2.Distance(_playerTransform.position, transform.position) <= MinDistanceForAggression)
        {
            SetState(TypeState.Attack);
            return;
        }

        if (Vector2.Distance(_playerTransform.position, transform.position) <= MinDistanceForAttack && _fighter.CanHitTarget())
        {
            SetState(TypeState.Attack);
        }
    }

    private void UpdateStateAttack()
    {
        if (Vector2.Distance(_playerTransform.position, transform.position) >= MaxDistanceForAttack)
        {
            SetState(TypeState.Patrol);
        }
    }

    private void SetState(TypeState typeState)
    {
        _typeState = typeState;
    }

    private void OnTookDamage()
    {
        _isTookDamage = true;
    }

    private void OnDied()
    {
        enabled = false;
        SetState(TypeState.None);
        _movement.enabled = false;
        _fighter.enabled = false;
        _health.enabled = false;
    }
}