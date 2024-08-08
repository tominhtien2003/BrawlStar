using Fusion;
using System;
using UnityEngine;

public class Trajectory : NetworkBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] float angle;
    [SerializeField] float gravity;
    [SerializeField] int numberOfPoints;
    [SerializeField] Transform pointShoot;

    private LineRenderer lineRenderer;
    public Vector3[] TrajectoryRocket;
    private Vector3[] points;
    private SwipeArea swipeHandler;

    public override void Spawned()
    {
        lineRenderer = GetComponent<LineRenderer>();

        swipeHandler = FindObjectOfType<SwipeArea>();

        if (swipeHandler != null)
        {
            swipeHandler.OnSwipe += HandleSwipe;
        }
        TrajectoryRocket = new Vector3[numberOfPoints];

        points = new Vector3[numberOfPoints];
    }
    private void FixedUpdate()
    {
        if (HasInputAuthority)
        {
            DrawTrajectory();
        }
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
        TrajectoryRocket = points;

        lineRenderer.SetPositions(points);
    }
}
