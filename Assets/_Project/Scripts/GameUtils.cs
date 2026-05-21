using UnityEngine;
using UnityEngine.InputSystem;

public static class GameUtils
{
    public static bool IsMouseClicked => Mouse.current.IsPressed();
}
