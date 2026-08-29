using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Dependencias")]
    [SerializeField] private PlayerAnimationManager animManager;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 movementVector;

    // Trackeamos el estado de movimiento localmente
    private bool isWalking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimationState();
    }

    private void HandleMovement()
    {
        movementVector = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 targetVelocity = movementVector * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    private void HandleRotation()
    {
        if (movementVector.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementVector);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimationState()
    {
        bool isMoving = movementVector.sqrMagnitude > 0.01f;

        // Solo llamamos al gestor si el estado de movimiento cambia
        if (isMoving && !isWalking)
        {
            animManager.ChangeState("Walk");
            isWalking = true;
        }
        else if (!isMoving && isWalking)
        {
            animManager.ChangeState("Idle");
            isWalking = false;
        }
    }
}