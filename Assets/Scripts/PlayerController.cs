using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    private StateMachine stateMachine;
    private Vector3 directionPlayer;
    private Animator animator;
    private NetworkCharacterController characterController;
    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        characterController = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        if (HasInputAuthority)
        {
            Camera.main.GetComponent<CameraFollowPlayer>().SetTarget(transform);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.direction.sqrMagnitude > 0f)
            {
                directionPlayer = data.direction;
            }
            if (data.direction != Vector3.zero)
            {
                stateMachine.ChangeState(new PlayerStateMove(this));
            }
            else
            {
                stateMachine.ChangeState(new PlayerStateIdle(this));
            }
        }
    }
    public void HandlePlayerMove()
    {
        characterController.Move(directionPlayer);

        animator.SetBool("Move", true);
    }
    public void HandlePlayerIdle()
    {
        characterController.Move(Vector3.zero);

        animator.SetBool("Move", false);
    }
}
