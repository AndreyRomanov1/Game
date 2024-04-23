using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BlockScript : MonoBehaviour
{
    public GridScript generator;

    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;

    [FormerlySerializedAs("entranceBlock")]
    public BlockScript entranceBlockScript;

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

    public Tilemap tilemap;
    public string EnemySpawnTileName = "tileset-sliced_119";

    public void Start()
    {
        tilemap = GetComponent<Tilemap>();
        var bounds = tilemap.cellBounds;
        for (var x = bounds.xMin; x < bounds.xMax; x++)
        for (var y = bounds.yMin; y < bounds.yMax; y++)
        {
            var pos = new Vector3Int(x, y, 0);
            var tile = tilemap.GetTile(pos);
            if (tile != null && tile.name == EnemySpawnTileName)
            {
                var tileWorldPos = tilemap.GetCellCenterWorld(new Vector3Int(x, y + 1, 0));
                var p = Resources.Load<GameObject>("Enemies/Barrel");
                var e = Instantiate(p, transform, true);
                e.transform.position = tileWorldPos;
            }
        }
    }
}