using UnityEngine;

public class GameInput : MonoBehaviour
{
    private InputActions inputActions;
    private void Start()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }
    public Vector2 GetMoveDirectionPlayer()
    {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
