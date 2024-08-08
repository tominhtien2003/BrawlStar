using Fusion;
using System;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] NetworkObject rocketPrefab;
    [SerializeField] Transform pointShoot;

    private Vector3 directionPlayer;
    private Animator animator;
    private Rigidbody rb;

    private Trajectory trajectory;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        trajectory = GetComponent<Trajectory>();
    }

    public override void Spawned()
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
            directionPlayer = transform.forward * data.direction.y + transform.right * data.direction.x;
            directionPlayer.Normalize();

            if (directionPlayer != Vector3.zero)
            {
                HandlePlayerMove();
            }
            else
            {
                HandlePlayerIdle();
            }
            //if (data.buttons.IsSet(NetworkInputData.BUTTONSHOOT))
            //{
            //    if (rocketPrefab != null)
            //    {
            //        HandlePlayerAttack();
            //    }
            //}
        }
    }

    public void HandlePlayerMove()
    {
        rb.velocity = directionPlayer * moveSpeed;
        transform.forward = Vector3.Lerp(transform.forward, directionPlayer, Runner.DeltaTime * 5f);
        animator.SetBool("Move", true);
    }

    public void HandlePlayerIdle()
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("Move", false);
    }

    public void HandlePlayerAttack()
    {
        if (Runner.IsServer) 
        {
            try
            {
                NetworkObject networkObject = Runner.Spawn(rocketPrefab, pointShoot.position, Quaternion.identity , Runner.LocalPlayer);

                if (networkObject != null)
                {
                    Rocket rocket = networkObject.GetComponent<Rocket>();

                    if (rocket != null)
                    {
                        Vector3[] copiedTrajectory = new Vector3[trajectory.TrajectoryRocket.Length];
                        Array.Copy(trajectory.TrajectoryRocket, copiedTrajectory, trajectory.TrajectoryRocket.Length);
                        rocket.SetTrajectoryRocket(copiedTrajectory);
                    }
                    else
                    {
                        Debug.LogWarning("Rocket component is missing on the spawned NetworkObject.");
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to spawn NetworkObject.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occurred while spawning NetworkObject: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Only server can spawn objects.");
        }
    }
}
