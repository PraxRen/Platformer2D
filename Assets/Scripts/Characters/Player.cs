using UnityEngine;

public class Player : Character
{
    [SerializeField] private PlayerInput _playerInput;

    private void Update()
    {
        Mover.Move(_playerInput.DirectionMove);
    }

    private void OnAttack()
    {
        if (Fighter.CanAttack())
            Fighter.Attack();
    }

    private void OnRun()
    {
        Mover.Run();
    }

    private void OnCancelRun()
    {
        Mover.CancelRun();
    }

    private void OnJump()
    {
        Mover.Jump();
    }

    protected override void HandleEnable()
    {
        _playerInput.Jump += OnJump;
        _playerInput.Run += OnRun;
        _playerInput.CancelRun += OnCancelRun;
        _playerInput.Attack += OnAttack;
    }

    protected override void HandleDisable()
    {
        _playerInput.Jump -= OnJump;
        _playerInput.Run -= OnRun;
        _playerInput.CancelRun -= OnCancelRun;
        _playerInput.Attack -= OnAttack;
    }
}