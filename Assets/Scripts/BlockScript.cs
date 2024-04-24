using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockScript : MonoBehaviour
{
    public GridScript generator;

    public BlockDirections entranceDirection;
    public BlockDirectionsNumbers entranceNumber;
    public BlockDirections exitDirection;
    public BlockDirectionsNumbers exitNumber;

    public BlockScript entranceBlockScript;

    public BlockScript exitBlockScript;

    public Tilemap tilemap;
    private const string EnemySpawnTileName = "tileset-sliced_119_1";
    private const string SomeEnemiesSpawnTileName = "tileset-sliced_119_3"; // TODO Поменять когда изменятся текстуры
    private const string BarrelSpawnTileName = "tileset-sliced_119";

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
                    break;
                case SomeEnemiesSpawnTileName:
                    break;
                case BarrelSpawnTileName:
                    var prefab = Resources.Load<GameObject>("Enemies/Barrel");
                    var position = tilemap.GetCellCenterWorld(new Vector3Int(x, y + 1, 0));
                    SpawnEnemy(prefab, position);
                    break;
            }
        }
    }

    private void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        var enemy = Instantiate(prefab, transform, true);
        enemy.transform.position = position;
    }
}