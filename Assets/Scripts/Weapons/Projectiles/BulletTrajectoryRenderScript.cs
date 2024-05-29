using UnityEngine;

public class BulletTrajectoryRenderScript : MonoBehaviour
{
    public int count = 10;
    public float deltaTime = 0.1f;

    private LayerMask trajectoryStoppers;
    private LineRenderer lineRendererComponent;

    private void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
        trajectoryStoppers = GetComponent<BaseWeaponScript>().mask;
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