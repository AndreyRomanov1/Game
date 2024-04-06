using UnityEngine;
using src.Model; 


public class Block : MonoBehaviour
{
    public GridScript generator;

    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;
    public Block entranceBlock;
    public Block exitBlock;

    public void Init(GridScript generator0, Vector3 position0, Block entranceBlock0,
        BlockDirections entranceDirection0, BlockDirectionsNumbers entranceNumber0,
        BlockDirections exitDirection0, BlockDirectionsNumbers exitNumber0)
    {
        generator = generator0;
        transform.position = position0;
        entranceBlock = entranceBlock0;
        entranceDirection = entranceDirection0;
        entranceNumber = entranceNumber0;
        exitDirection = exitDirection0;
        exitNumber = exitNumber0;
    }
}