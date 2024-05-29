using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockScript : MonoBehaviour
{
    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;

    public BlockScript entranceBlockScript;

    public BlockScript exitBlockScript;

    public Tilemap tilemap;
    private const string EnemySpawnTileName = "камень с врагом";
    private const string SomeEnemiesSpawnTileName = "камень с врагами";
    private const string BarrelSpawnTileName = "камень с бочкой";

    public void Init(GridScript generator0, Vector3 position0, BlockScript entranceBlock0,
        BlockDirections entranceDirection0, BlockDirectionsNumbers entranceNumber0,
        BlockDirections exitDirection0, BlockDirectionsNumbers exitNumber0)
    {
        transform.position = position0;
        entranceBlockScript = entranceBlock0;
        entranceDirection = entranceDirection0;
        entranceNumber = entranceNumber0;
        exitDirection = exitDirection0;
        exitNumber = exitNumber0;
    }

    public void Start()
    {
        tilemap = GetComponent<Tilemap>();
        SpawnEnemiesOnNeedTile();
    }

    private void SpawnEnemiesOnNeedTile()
    {
        var bounds = tilemap.cellBounds;
        for (var x = bounds.xMin; x < bounds.xMax; x++)
        for (var y = bounds.yMin; y < bounds.yMax; y++)
        {
            var tilePosition = new Vector3Int(x, y, 0);
            var tile = tilemap.GetTile(tilePosition);
            if (tile == null)
                continue;
            switch (tile.name)
            {
                case EnemySpawnTileName:
                {
                    var position = tilemap.GetCellCenterWorld(new Vector3Int(x, y + 1, 0));
                    Tools.SpawnEnemy(position, gameObject);
                    break;
                }
                case SomeEnemiesSpawnTileName:
                {
                    var position = tilemap.GetCellCenterWorld(new Vector3Int(x, y + 1, 0));
                    Tools.SpawnEnemy(position, gameObject);
                    break;
                }
                case BarrelSpawnTileName:
                {
                    var prefab = Resources.Load<GameObject>("Enemies/Objects/Barrel");
                    var position = tilemap.GetCellCenterWorld(new Vector3Int(x, y + 1, 0));
                    Tools.SpawnObject(prefab, position, gameObject);
                    break;
                }
            }
        }
    }
}