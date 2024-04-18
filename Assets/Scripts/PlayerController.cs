using UnityEngine;

[RequireComponent(typeof(Movement), typeof(Fighter), typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Movement _movement;
    private Fighter _fighter;
    private Health _health;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _fighter = GetComponent<Fighter>();
        _health = GetComponent<Health>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
        _playerInput.Jump += OnJump;
        _playerInput.Run += OnRun;
        _playerInput.CancelRun += OnCancelRun;
        _playerInput.Attack += OnAttack;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
        _playerInput.Jump -= OnJump;
        _playerInput.Run -= OnRun;
        _playerInput.CancelRun -= OnCancelRun;
        _playerInput.Attack -= OnAttack;
    }

    private void FixedUpdate()
    {
        _movement.Move(_playerInput.DirectionMove);
    }

    private void OnAttack()
    {
        if (_fighter.CanAttack())
            _fighter.Attack();
    }

    private void OnRun()
    {
        _movement.Run();
    }

    private void OnCancelRun()
    {
        _movement.CancelRun();
    }

    private void OnJump()
    {
        _movement.Jump();
    }

    private void OnDied()
    {
        enabled = false;
        _movement.enabled = false;
        _fighter.enabled = false;
        _health.enabled = false;
    }
}
