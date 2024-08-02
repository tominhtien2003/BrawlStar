using Fusion;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private InputActions inputActions;
    private PointerDetector pointerDetector;
    [Networked] private bool isPressShootButton { get; set; }
    private void Start()
    {
        inputActions = new InputActions();

        inputActions.Player.Enable();

        PointerDetector pointerDetector = GameObject.Find("ButtonShoot").GetComponent<PointerDetector>();

        pointerDetector.OnButtonShootPress += PointerDetector_OnButtonShootPress;
    }
    private void PointerDetector_OnButtonShootPress(object sender, PointerDetector.OnButtonShootPressEventArgs e)
    {
        isPressShootButton = e.isPress;
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
