using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private float _jumpHeight = 2f;

    [SerializeField] private float _boostedSpeed = 8f;
    [SerializeField] private float _boostDuration = 5f;

    private CharacterController _controller;
    private Animator _animator;
    private float _verticalVel;
    private bool _wasGrounded;
    private bool _isJumping;
    private float _normalSpeed;
    private Coroutine _boostRoutine;

    private const float deadZone = 0.01f;
    private const float dampTime = 0.1f;
    private const float skinGuard = -2f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _normalSpeed = _moveSpeed;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnJump += OnJump;
        CollectManager.Instance.OnBonusCollected += OnBonusCollected;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnJump -= OnJump;
        CollectManager.Instance.OnBonusCollected -= OnBonusCollected;
    }

    private void Update()
    {
        MovementAndRotation();
        GravityAndLanding();
        UpdateAnimatorParameters();
    }
    private void OnJump()
    {
        if (IsGrounded())
        {
            _verticalVel = Mathf.Sqrt(2f * _jumpHeight * 9.81f);
            _isJumping = true;
            _animator.SetTrigger("Jump");
            AudioManager.Instance.PlayJump();
        }
    }

    private void OnBonusCollected(BonusType type, int count)
    {
        if (type != BonusType.SpeedBoost) return;
        RestartCoroutine(ref _boostRoutine, SpeedBoostCoroutine());
    }

    private void MovementAndRotation()
    {
        Vector2 inp = InputManager.Instance.MoveInput;
        if (inp.sqrMagnitude < deadZone) return;

        Transform cam = Camera.main.transform;
        Vector3 forward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = cam.right;
        Vector3 dir = forward * inp.y + right * inp.x;

        float targetY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float smoothedY = Mathf.LerpAngle(transform.eulerAngles.y, targetY, _rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, smoothedY, 0);

        _controller.Move(dir * _moveSpeed * Time.deltaTime);
    }

    private void GravityAndLanding()
    {
        bool groundedNow = IsGrounded();

        if (groundedNow && _verticalVel < 0f)
            _verticalVel = skinGuard;

        _verticalVel -= 9.81f * Time.deltaTime;
        _controller.Move(Vector3.up * _verticalVel * Time.deltaTime);

        if (groundedNow && !_wasGrounded && _isJumping && _verticalVel <= 0f)
        {
            _animator.SetTrigger("Land");
            _isJumping = false;
        }

        _wasGrounded = groundedNow;
    }
    private void UpdateAnimatorParameters()
    {
        Vector2 inp = InputManager.Instance.MoveInput;
        float rawV = inp.y, rawH = inp.x;
        float v = Mathf.Abs(rawV) < deadZone ? 0f : rawV;
        float h = Mathf.Abs(rawH) < deadZone ? 0f : rawH;

        _animator.SetFloat("Vertical", v, dampTime, Time.deltaTime);
        _animator.SetFloat("Horizontal", h, dampTime, Time.deltaTime);
    }
    private bool IsGrounded()
    {
        return (_controller.collisionFlags & CollisionFlags.Below) != 0;
    }
    private IEnumerator SpeedBoostCoroutine()
    {
        _moveSpeed = _boostedSpeed;
        yield return new WaitForSeconds(_boostDuration);
        _moveSpeed = _normalSpeed;
    }
    private void RestartCoroutine(ref Coroutine routine, IEnumerator routineBody)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(routineBody);
    }
}
