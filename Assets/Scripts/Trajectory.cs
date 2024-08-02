using Fusion;
using System;
using UnityEngine;

public class Trajectory : NetworkBehaviour
{
    public float velocity;
    public float angle;
    public float gravity;
    public int numberOfPoints;
    public Transform pointShoot;

    private LineRenderer lineRenderer;
    private Vector3[] trajectoryRocket;
    private Vector3[] points;
    [SerializeField] private NetworkObject rocketPrefab;
    private SwipeArea swipeHandler;

    private Rocket rocket;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        swipeHandler = FindObjectOfType<SwipeArea>();
        if (swipeHandler != null)
        {
            swipeHandler.OnSwipe += HandleSwipe;
        }
        trajectoryRocket = new Vector3[numberOfPoints];
        points = new Vector3[numberOfPoints];
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            bool isEditingForceShoot = data.buttons.IsSet(NetworkInputData.BUTTONEDITFORCESHOOT);

            SetLineRendererAlpha(isEditingForceShoot ? 1f : 0f);

            DrawTrajectory();

            if (data.buttons.IsSet(NetworkInputData.BUTTONSHOOT) && rocket == null)
            {
                Debug.Log("aa");
                if (Runner.IsServer && rocketPrefab != null)
                {
                    NetworkObject networkObject = Runner.Spawn(rocketPrefab, pointShoot.position, Quaternion.identity);
                    if (networkObject != null)
                    {
                        rocket = networkObject.GetComponent<Rocket>();
                        if (rocket != null)
                        {
                            // Sao chép mảng trajectoryRocket trước khi truyền cho rocket
                            Vector3[] copiedTrajectory = new Vector3[trajectoryRocket.Length];
                            Array.Copy(trajectoryRocket, copiedTrajectory, trajectoryRocket.Length);
                            rocket.SetTrajectoryRocket(copiedTrajectory);
                            rocket.transform.SetParent(null);
                        }
                    }
                }
            }
        }
    }

    private void SetLineRendererAlpha(float alpha)
    {
        if (Mathf.Approximately(lineRenderer.startColor.a, alpha) && Mathf.Approximately(lineRenderer.endColor.a, alpha))
        {
            return;
        }

        Color startColor = lineRenderer.startColor;
        startColor.a = alpha;
        lineRenderer.startColor = startColor;

        Color endColor = lineRenderer.endColor;
        endColor.a = alpha;
        lineRenderer.endColor = endColor;
    }

    private void HandleSwipe(Vector3 mouseDirection)
    {
        velocity += mouseDirection.y > 0 ? 0.1f : -0.1f;
        velocity = Mathf.Clamp(velocity, 5f, 12f);
    }

    private void DrawTrajectory()
    {
        lineRenderer.positionCount = numberOfPoints;

        Vector3 startPos = pointShoot.position;
        float radianAngle = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radianAngle);
        float sin = Mathf.Sin(radianAngle);
        float t_total = 2 * velocity * sin / -gravity;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * t_total / (numberOfPoints - 1);
            float x = velocity * cos * t;
            float y = velocity * sin * t + 0.5f * gravity * t * t;

            points[i] = startPos + transform.forward * x + Vector3.up * y;
        }
        trajectoryRocket = points;

        lineRenderer.SetPositions(points);
    }

    public Vector3[] GetTrajectory()
    {
        return trajectoryRocket;
    }
}
