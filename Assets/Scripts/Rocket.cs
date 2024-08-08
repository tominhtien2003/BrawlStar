using Fusion;
using System.Collections;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    [SerializeField] private NetworkObject explosionVFX;
    private Vector3[] trajectoryRocket;
    public float speed = 10f;
    private float vfxLifetime = 0.8f;
    public void SetTrajectoryRocket(Vector3[] trajectoryRocket)
    {
        this.trajectoryRocket = trajectoryRocket;
        Shooting();
    }

    private void Shooting()
    {
        if (trajectoryRocket != null && trajectoryRocket.Length > 1)
        {
            StartCoroutine(MoveAlongTrajectory());
        }
    }

    private IEnumerator MoveAlongTrajectory()
    {
        for (int i = 0; i < trajectoryRocket.Length - 1; i++)
        {
            Vector3 start = trajectoryRocket[i];
            Vector3 end = trajectoryRocket[i + 1];

            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            float travelTime = distance / speed;
            float elapsedTime = 0f;

            while (elapsedTime < travelTime)
            {
                transform.position = Vector3.Lerp(start, end, elapsedTime / travelTime);
                transform.rotation = Quaternion.LookRotation(direction);

                elapsedTime += Runner.DeltaTime;
                yield return null;
            }

            transform.position = end;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Runner.IsServer)
        {
            NetworkObject vfxInstance = Runner.Spawn(explosionVFX, transform.position, Quaternion.identity);
            StartCoroutine(DestroyVFXAfterDelay(vfxInstance));
        }
    }

    private IEnumerator DestroyVFXAfterDelay(NetworkObject vfx)
    {
        yield return new WaitForSeconds(vfxLifetime);

        if (Runner.IsServer && vfx != null)
        {
            Runner.Despawn(vfx);
            StartCoroutine(DestroyAfterTime());
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(0.1f);
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
