using UnityEngine;

public class Block : MonoBehaviour
{
    public Grid generator;
    public int x;
    public int y;

    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;
    public Block entranceBlock;
    public Block exitBlock;

    public void Init(Block entranceBlock0,
        BlockDirections entranceDirection0, BlockDirectionsNumbers entranceNumber0,
        BlockDirections exitDirection0, BlockDirectionsNumbers exitNumber0)
    {
        entranceBlock = entranceBlock0;
        entranceDirection = entranceDirection0;
        entranceNumber = entranceNumber0;
        exitDirection = exitDirection0;
        exitNumber = exitNumber0;
    }

    private void Start()
    {
        generator = GetComponentInParent<Grid>();
        var position = transform.position;
        x = (int)position.x / generator.blockWidth;
        y = (int)position.y / generator.blockHeight;
        GenerateIfNeedNextLevelBlock();
    }

    private void Update()
    {
        GenerateIfNeedNextLevelBlock();

        var d = (generator.player.transform.position - transform.position).magnitude;
        if (d < generator.emptyGenerationRadius)
            for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
                if (!generator.Map.ContainsKey((x + i, y + j)))
                    generator.InitEmptyBlock(
                        new Vector3(generator.blockWidth * (x + i), generator.blockHeight * (y + j), 0)
                    );
    }

    private void GenerateIfNeedNextLevelBlock()
    {
        var d = (generator.player.transform.position - transform.position).magnitude;
        if (d < generator.levelGenerationRadius && exitBlock == null)
        {
            var i = exitDirection == BlockDirections.Right ? 1 : 0;
            var j = exitDirection == BlockDirections.Up ? 1 : exitDirection == BlockDirections.Down ? -1 : 0;
            if (generator.Map.ContainsKey((x + i, y + j)))
                Destroy(generator.Map[(x + i, y + j)]);

            generator.InitLevelBlock(new Vector3(generator.blockWidth * (x + i),
                generator.blockHeight * (y + j)), exitDirection, exitNumber, this);
        }
    }
}