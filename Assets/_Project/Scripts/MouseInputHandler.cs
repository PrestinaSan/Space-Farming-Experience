using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHandler : MonoBehaviour
{
    public static MouseInputHandler Instance;
    private void Awake()
    {
        Instance = this;
    }
    public bool clicked;
    public Vector2 position;

    public void OnMouseClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            clicked = true;
            position = Mouse.current.position.ReadValue();
        }
        if (ctx.canceled)
        {
            clicked = false;
        }
    }
}
