using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BulletTrajectoryRenderScript : MonoBehaviour
{
    public int count = 10;
    public float deltaTime = 0.1f;
    
    private LayerMask trajectoryStoppers;
    private LineRenderer lineRendererComponent;

    void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
        trajectoryStoppers = GetComponent<PistolScript>().mask;
        Debug.Log(lineRendererComponent);
    }

    public void ShowTrajectory()
    {
        var result = new Vector3[count];
        lineRendererComponent.positionCount = count;
        var direction = transform.rotation * new Vector3(1, 0);
        
        result[0] = transform.position;
        
        for (var i = 1; i < count; i++)
        {
            var time = deltaTime * i;
            result[i] = transform.position + direction * time;

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