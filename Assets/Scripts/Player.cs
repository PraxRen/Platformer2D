using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Fighter), typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Mover _mover;
    private Fighter _fighter;
    private Health _health;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
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

    private void Update()
    {
        _mover.Move(_playerInput.DirectionMove);
    }

    private void OnAttack()
    {
        if (_fighter.CanAttack())
            _fighter.Attack();
    }

    private void OnRun()
    {
        _mover.Run();
    }

    private void OnCancelRun()
    {
        _mover.CancelRun();
    }

    private void OnJump()
    {
        _mover.Jump();
    }

    private void OnDied()
    {
        enabled = false;
        _mover.enabled = false;
        _fighter.enabled = false;
        _health.enabled = false;
    }
}
