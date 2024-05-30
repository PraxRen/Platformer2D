using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Fighter), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private static int s_idLast;

    [SerializeField] private Health _health;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Mover _mover;
    [SerializeField] private Fighter _fighter;
    [SerializeField] private GameObject _ui;

    public int Id { get; private set; }

    private void Awake()
    {
        Id = ++s_idLast;
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
        _ui.SetActive(false);
    }
}
