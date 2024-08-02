using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
    private InputActions inputActions;
    [Networked] public bool isPressShootButton { get; set; }
    [Networked] private bool isEditForceButton { get; set; }
    private void Start()
    {
        inputActions = new InputActions();

        inputActions.Player.Enable();

        isPressShootButton = false;
    }
    public void ShootButton()
    {
        isPressShootButton = true;
    }
    public void EditButton()
    {
        isEditForceButton = !isEditForceButton;
    }
    public bool IsPressShootButton()
    {
        return isPressShootButton;
    }
    public bool IsEditForceButton()
    {
        return isEditForceButton;
    }
    public Vector2 GetMoveDirectionPlayer()
    {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
