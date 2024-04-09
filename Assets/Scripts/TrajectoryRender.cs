using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrajectoryRender : MonoBehaviour
{
    public int count = 10;
    public float deltaTime = 0.1f;
    public LayerMask trajectoryStoppers;
    
    private LineRenderer lineRendererComponent;
    private Rigidbody2D physics;

    void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
        physics = GetComponentInParent<Rigidbody2D>();
    }

    public void ShowTrajectory(Vector2 force)
    {
        var result = new Vector3[count];
        lineRendererComponent.positionCount = count;

        var boost = force / physics.mass;
        var velocity = boost * Time.fixedDeltaTime;
        result[0] = transform.position;
        
        for (var i = 1; i < count; i++)
        {
            var time = deltaTime * i;
            result[i] = transform.position +
                        (Vector3)(velocity * time + Physics2D.gravity * (time * time * physics.gravityScale) / 2f);

            if (Tools.FindObjectOnLine(result[i - 1], result[i],
                    trajectoryStoppers, out var other))
            {
                lineRendererComponent.positionCount = i;
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
