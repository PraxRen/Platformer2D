using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private enum Direction
    {
        Right,
        Left
    }

    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRun;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundedOffset;
    [SerializeField] private float _groundedRadius;
    [SerializeField] private float _fallTimeout;
    [SerializeField] private float _jumpLimitTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _graphics;

    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _collider;
    private bool _isGrounded;
    private bool _isJump;
    private bool _isFreeFall;
    private bool _isRun;
    private float _currentFallTimeout;
    private float _currentJumpTime;
    private float _targetSpeed;
    private Direction _currentLookDirection;
    private Vector2 _targetDirectionMove;

    private void OnEnable()
    {
        if (_rigidbody2D != null)
        {
            _collider.enabled = true;
            _rigidbody2D.isKinematic = false;
        }
    }

    private void OnDisable()
    {
        _rigidbody2D.isKinematic = true;
        _collider.enabled = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _currentFallTimeout = _fallTimeout;
    }

    private void FixedUpdate()
    {
        UpdateCollisionGround();
        HandleMove();
        HandleJump();
        HandleFreeFall();
        UpdateAnimations();
    }

    public void Jump()
    {
        if (_isGrounded == false)
            return;

        if (_isJump)
            return;

        _isJump = true;
        _currentJumpTime = 0;
    }

    public void Run()
    {
        if (_isGrounded == false)
            return;

        _isRun = true;
    }

    public void CancelRun()
    {
        _isRun = false;
    }

    public void Move(Vector2 direction)
    {
        _targetDirectionMove = direction;
    }

    public void LookAtDirection(Vector2 direction)
    {
        Direction targetDirection = direction.x < 0 ? Direction.Left : Direction.Right;

        if (_currentLookDirection == targetDirection)
            return;

        _currentLookDirection = targetDirection;
        _graphics.localScale = new Vector3(_graphics.localScale.x * -1, _graphics.localScale.y, _graphics.localScale.z);
    }

    private void UpdateCollisionGround()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + _groundedOffset, transform.position.z);
        _isGrounded = Physics2D.OverlapCircle(spherePosition, _groundedRadius, _groundLayer);
    }

    private void HandleMove()
    {
        if (_targetDirectionMove.magnitude == 0)
        {
            _targetSpeed = 0;
            return;
        }

        if (_isRun)
            _targetSpeed = _speedRun;
        else
            _targetSpeed = _speedWalk;

        LookAtDirection(_targetDirectionMove);
        Vector2 targetVelocity = _targetDirectionMove.normalized * _targetSpeed;
        targetVelocity.y = _rigidbody2D.velocity.y;
        _rigidbody2D.velocity = targetVelocity;
    }

    private void HandleJump()
    {
        if (_isJump == false)
            return;

        if (_currentJumpTime == 0)
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);

        _currentJumpTime += Time.deltaTime;

        if (_currentJumpTime <= _jumpLimitTime)
            return;

        _isJump = false;
    }

    private void HandleFreeFall()
    {
        if (_isGrounded)
        {
            _isFreeFall = false;
            _currentFallTimeout = _fallTimeout;
        }
        else
        {
            _currentFallTimeout -= Time.deltaTime;
        }

        if (_currentFallTimeout >= 0f)
            return;

        _isFreeFall = true;
    }

    private void UpdateAnimations()
    {
        _animator.SetFloat(CharacterAnimatorData.Params.Speed, _targetSpeed);
        _animator.SetBool(CharacterAnimatorData.Params.IsFreeFall, _isFreeFall);
        _animator.SetBool(CharacterAnimatorData.Params.IsJump, _isJump);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = Color.green;
        Color transparentRed = Color.red;
        Gizmos.color = _isGrounded ? transparentGreen : transparentRed;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + _groundedOffset, transform.position.z), _groundedRadius);
    }
}