using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Fighter))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    private Movement _movement;
    private Fighter _fighter;
    private Health _health;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (_health != null)
            _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _fighter = GetComponent<Fighter>();
        _health = GetComponent<Health>();
        _health.Died += OnDied;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        _movement.Move(new Vector2(moveInput, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movement.Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movement.Run();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _movement.CancelRun();
        }

        if (Input.GetMouseButtonDown(0) && _fighter.CanAttack())
        {
            _fighter.Attack();
        }
    }

    private void OnDied()
    {
        enabled = false;
        _movement.enabled = false;
        _fighter.enabled = false;
        _health.enabled = false;
    }
}
