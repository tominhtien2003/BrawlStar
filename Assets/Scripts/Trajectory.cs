using Fusion;
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
    [SerializeField] NetworkObject rocketPrefab;
    [Networked] bool waitShoot  {get;set;}

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.buttons.IsSet(NetworkInputData.BUTTONSHOOT))
            {
                waitShoot = true;
                DrawTrajectory();
            }
            else
            {
                if (waitShoot)
                {
                    waitShoot = false;
                    lineRenderer.positionCount = 0;
                    if (Runner.IsServer && rocketPrefab != null)
                    {
                        NetworkObject networkObject = Runner.Spawn(rocketPrefab, pointShoot.position, Quaternion.identity);
                        if (networkObject != null)
                        {
                            Rocket rocket = networkObject.gameObject.GetComponent<Rocket>();
                            if (rocket != null)
                            {
                                rocket.SetTrajectoryRocket(trajectoryRocket);
                            }
                        }
                    }
                }
            }
        }    
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
