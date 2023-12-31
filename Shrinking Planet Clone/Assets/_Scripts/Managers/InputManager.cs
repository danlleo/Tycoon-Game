using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public bool IsMouseButtonDownThisFrame() => Input.GetMouseButtonDown(0);

    public bool IsRightMouseButtonDownThisFrame() => Input.GetMouseButtonDown(1);

    public bool IsPauseButtonDownThisFrame() => Input.GetKeyDown(KeyCode.Escape);

    public Vector2 GetMouseScreenPosition() => Input.mousePosition;
}
