using Fusion;
using UnityEngine;

public class Trajectory : NetworkBehaviour
{
    public float velocity = 10f; // Vận tốc bắn
    public float angle = 45f; // Góc bắn
    public float gravity = -9.81f; // Trọng lực
    public int numberOfPoints = 30; // Số điểm trên đường đi
    public Transform pointShoot; // Điểm bắt đầu đường đi

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void FixedUpdateNetwork()
    {
        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        lineRenderer.positionCount = numberOfPoints;
        Vector3[] points = new Vector3[numberOfPoints];

        Vector3 startPos = pointShoot.position;
        float radianAngle = angle * Mathf.Deg2Rad;
        float t_total = 2 * velocity * Mathf.Sin(radianAngle) / -gravity;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * t_total / (numberOfPoints - 1);
            float x = velocity * Mathf.Cos(radianAngle) * t;
            float y = velocity * Mathf.Sin(radianAngle) * t + 0.5f * gravity * t * t;

            points[i] = startPos + new Vector3(x * transform.forward.x, y, 0);
        }
        lineRenderer.SetPositions(points);
    }
}
