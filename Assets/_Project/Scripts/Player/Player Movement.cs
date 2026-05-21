using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]

    [SerializeField] private float baseMovementSpeed;
    private float movementSpeed;

    public float BaseMovementSpeed => baseMovementSpeed;


    private Vector2 _moveDirection;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _moveDirection = ctx.ReadValue<Vector2>();
        }

        if (ctx.canceled)
        {
            _moveDirection = Vector2.zero;
        }
    }

    void Start()
    {
        movementSpeed = baseMovementSpeed;
    }


    void FixedUpdate()
    {
        MovementInput();
    }
    private void MovementInput()
    {
        //float xInput = Input.GetAxis("Horizontal");
        //float yInput = Input.GetAxis("Vertical");
        //Vector2 direction = new Vector2(xInput, yInput).normalized;

        rb.linearVelocity = _moveDirection * movementSpeed;
    }
    public void ChangeMovementSpeed(float newMovementSpeed)
    {
        movementSpeed = newMovementSpeed;
    }
    public void ResetMovementSpeed()
    {
        movementSpeed = baseMovementSpeed;
    }

}
