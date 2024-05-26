using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _buttonJump;
    [SerializeField] private KeyCode _buttonRun;
    [SerializeField] private KeyCode _buttonAttack;

    public event Action Jump;
    public event Action Run;
    public event Action CancelRun;
    public event Action Attack;

    public Vector2 DirectionMove { get; private set; }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        DirectionMove = new Vector2(moveInput, 0);

        if (Input.GetKeyDown(_buttonJump))
        {
            Jump?.Invoke();
        }

        if (Input.GetKey(_buttonRun))
        {
            Run?.Invoke();
        }
        else if (Input.GetKeyUp(_buttonRun))
        {
            CancelRun?.Invoke();
        }

        if (Input.GetKeyDown(_buttonAttack))
        {
            Attack?.Invoke();
        }
    }
}