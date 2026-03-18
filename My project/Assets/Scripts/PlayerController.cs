using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Force applied to move the ball (units/s^2). Use ForceMode.Acceleration for consistent feel.")]
    public float moveForce = 10f;

    [Tooltip("Maximum horizontal speed (m/s). Y velocity (falling/jump) is preserved).")]
    public float maxSpeed = 6f;

    [Header("References")]
    [Tooltip("Rigidbody attached to the player (will be fetched automatically if empty).")]
    public Rigidbody rb;

    [Tooltip("If set, the script will use this PlayerInput's action asset and look for the action named 'Player/Move'.")]
    public UnityEngine.InputSystem.PlayerInput playerInput;

    [Tooltip("Alternative: assign the Move action directly from the Input Actions asset (recommended for simplicity).")]
    public InputActionReference moveActionReference;

    // runtime reference to the active move action
    private InputAction moveAction;

    void Reset()
    {
        // try to get existing Rigidbody on same GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Prefer explicit moveActionReference when provided
        if (moveActionReference != null && moveActionReference.action != null)
        {
            moveAction = moveActionReference.action;
        }
        else if (playerInput != null && playerInput.actions != null)
        {
            // Find the action by full path "Player/Move". This is robust when the PlayerInput is configured
            // to use the InputAction asset that contains the map named "Player" with an action named "Move".
            moveAction = playerInput.actions.FindAction("Player/Move", true);
            if (moveAction == null)
            {
                // fallback: try to find action named "Move" anywhere
                moveAction = playerInput.actions.FindAction("Move", false);
            }
        }

        if (moveAction != null)
        {
            moveAction.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        Vector2 input = Vector2.zero;
        if (moveAction != null)
        {
            input = moveAction.ReadValue<Vector2>();
        }

        // Convert input (x, y) -> world (x, 0, y)
        Vector3 move = new Vector3(input.x, 0f, input.y);

        if (move.sqrMagnitude > 0f)
        {
            // Apply movement as acceleration so it feels consistent regardless of mass
            rb.AddForce(move * moveForce, ForceMode.Acceleration);
        }

        LimitHorizontalVelocity();
    }

    private void LimitHorizontalVelocity()
    {
        Vector3 vel = rb.linearVelocity;
        Vector3 horizontal = new Vector3(vel.x, 0f, vel.z);
        float speed = horizontal.magnitude;
        if (speed > maxSpeed)
        {
            Vector3 limited = horizontal.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, vel.y, limited.z);
        }
    }

    // Optional helper to set moveAction at runtime (e.g., if you generate actions via code)
    public void SetMoveAction(InputAction action)
    {
        if (moveAction == action) return;
        if (moveAction != null) moveAction.Disable();
        moveAction = action;
        if (moveAction != null && enabled) moveAction.Enable();
    }
}

