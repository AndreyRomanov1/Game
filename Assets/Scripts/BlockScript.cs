using UnityEngine;
using UnityEngine.Serialization;

public class BlockScript : MonoBehaviour
{
    public GridScript generator;

    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;
    [FormerlySerializedAs("entranceBlock")] public BlockScript entranceBlockScript;
    [FormerlySerializedAs("exitBlock")] public BlockScript exitBlockScript;

    public void Init(GridScript generator0, Vector3 position0, BlockScript entranceBlock0,
        BlockDirections entranceDirection0, BlockDirectionsNumbers entranceNumber0,
        BlockDirections exitDirection0, BlockDirectionsNumbers exitNumber0)
    {
        generator = generator0;
        transform.position = position0;
        entranceBlockScript = entranceBlock0;
        entranceDirection = entranceDirection0;
        entranceNumber = entranceNumber0;
        exitDirection = exitDirection0;
        exitNumber = exitNumber0;
    }
}