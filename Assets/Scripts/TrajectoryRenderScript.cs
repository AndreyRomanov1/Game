using UnityEngine;

public class TrajectoryRenderScript : MonoBehaviour
{
    private const int Count = 100;
    private const float DeltaTime = 0.1f;
    public LayerMask trajectoryStoppers;

    private LineRenderer lineRendererComponent;
    private Rigidbody2D physics;

    private void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
        physics = GetComponentInParent<Rigidbody2D>();
    }

    public void ShowTrajectory(Vector2 force)
    {
        var result = new Vector3[Count];
        lineRendererComponent.positionCount = Count;

        var boost = force / physics.mass;
        var velocity = boost * Time.fixedDeltaTime;
        result[0] = transform.position;

        for (var i = 1; i < Count; i++)
        {
            var time = DeltaTime * i;
            result[i] = transform.position
                        + (Vector3)(velocity * time + Physics2D.gravity * (time * time * physics.gravityScale) / 2f);

            if (Tools.FindObjectOnLine(result[i - 1], result[i],
                    trajectoryStoppers, out var other))
            {
                lineRendererComponent.positionCount = i + 1;
                break;
            }
        }

        lineRendererComponent.SetPositions(result);
    }

    public void ClearTrajectory()
    {
        lineRendererComponent.positionCount = 0;
    }
}