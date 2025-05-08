using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public event Action OnJump;

    public event Action OnRestart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _controls = new PlayerControls();
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Jump.performed += _ => OnJump?.Invoke();

        _controls.UI.Enable();
        _controls.UI.Restart.performed += _ => OnRestart?.Invoke();
    }

    private void OnDisable()
    {
        _controls.Player.Jump.performed -= _ => OnJump?.Invoke();
        _controls.Player.Disable();

        _controls.UI.Restart.performed -= _ => OnRestart?.Invoke();
        _controls.UI.Disable();
    }

    private void Update()
    {
        MoveInput = _controls.Player.Move.ReadValue<Vector2>();
        LookInput = _controls.Player.Look.ReadValue<Vector2>();
    }
}
