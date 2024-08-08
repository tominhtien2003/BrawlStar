using Fusion;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private InputActions inputActions;
    [Networked] public bool isPressShootButton { get; set; }

    private void Start()
    {
        inputActions = new InputActions();

        inputActions.Player.Enable();

        isPressShootButton = false;
    }
    public void ShootButton()
    {
        isPressShootButton = !isPressShootButton;
    }
    public bool IsPressShootButton()
    {
        return isPressShootButton;
    }
    public Vector2 GetMoveDirectionPlayer()
    {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
