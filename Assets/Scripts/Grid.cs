using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Grid : MonoBehaviour
{
    public GameObject playerPrefab;
    public int blockWidth = 32;
    public int blockHeight = 24;
    public Player player;
    public float levelGenerationRadius = 90;
    public float emptyGenerationRadius = 40;

    public readonly Dictionary<(int x, int y), GameObject> Map = new();

    private readonly Dictionary<BlockDirections, Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>> dir = new()
    {
        [BlockDirections.Down] = new Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>
        {
            [BlockDirectionsNumbers.First] = new(),
            [BlockDirectionsNumbers.Second] = new(),
            [BlockDirectionsNumbers.Third] = new()
        },
        [BlockDirections.Up] = new Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>
        {
            [BlockDirectionsNumbers.First] = new(),
            [BlockDirectionsNumbers.Second] = new(),
            [BlockDirectionsNumbers.Third] = new()
        },
        [BlockDirections.Right] = new Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>
        {
            [BlockDirectionsNumbers.First] = new(),
            [BlockDirectionsNumbers.Second] = new()
        }
    };

    public GameObject[] emptyPrefabs;
    private readonly Random random = new();


    private void Start()
    {
        LoadBlockPrefabs();
        InitLevelBlock(Vector3.zero, BlockDirections.Right, BlockDirectionsNumbers.First, null);
        InitPlayer();
    }

    private void Update()
    {
        var playerPosition = player.transform.position;
        var playerTilemapPosition = player.playerTilemap.transform.position;
        var dX = (int)(playerPosition.x - playerTilemapPosition.x);
        var dY = (int)(playerPosition.y - playerTilemapPosition.y);
        var mapX = (int)playerTilemapPosition.x / blockWidth;
        var mapY = (int)playerTilemapPosition.y / blockHeight;
        var i = dX > blockWidth / 2 ? 1 : dX < -blockWidth / 2 ? -1 : 0;
        var j = dY > blockHeight / 2 ? 1 : dY < -blockHeight / 2 ? -1 : 0;
        player.playerTilemap = Map[(mapX + i, mapY + j)];
    }

    public void InitEmptyBlock(Vector3 position)
    {
        var prefab = emptyPrefabs[random.Next(emptyPrefabs.Length)];
        var block = Instantiate(prefab, transform, true);
        block.transform.position = position;
        Map[((int)position.x / blockWidth, (int)position.y / blockHeight)] = block;
    }

    public void InitLevelBlock(Vector3 position, BlockDirections entranceDirection,
        BlockDirectionsNumbers entranceNumber, Block entranceBlock)
    {
        var prefabList = dir[entranceDirection][entranceNumber];
        var prefab = prefabList[random.Next(prefabList.Count)];
        var block = Instantiate(prefab.Prefab, transform, true);
        block.transform.position = position;
        var blockScript = block.GetComponent<Block>();
        blockScript.Init(entranceBlock, entranceDirection, entranceNumber, prefab.ExitDirection, prefab.ExitNumber);
        if (entranceBlock != null)
            entranceBlock.exitBlock = blockScript;

        Map[((int)position.x / blockWidth, (int)position.y / blockHeight)] = block;
    }

    private void InitPlayer()
    {
        var playerGameObject = Instantiate(playerPrefab);
        player = playerGameObject.GetComponent<Player>();
        var pos = player.transform.position;
        player.playerTilemap = Map[((int)pos.x / blockWidth, (int)pos.y / blockHeight)];
    }

    private void LoadBlockPrefabs()
    {
        var paths = new[]
        {
            "DownEntrance/1", "DownEntrance/2", "DownEntrance/3",
            "UpEntrance/1", "UpEntrance/2", "UpEntrance/3",
            "RightEntrance/1", "RightEntrance/2"
        };
        foreach (var path in paths)
        {
            var prefabs = Resources.LoadAll<GameObject>($"BlockPrefabs/{path}");
            foreach (var prefab in prefabs)
            {
                var g = new BlockGameObject(prefab);
                dir[g.EntranceDirection][g.EntranceNumber].Add(g);
            }
        }

        emptyPrefabs = Resources.LoadAll<GameObject>($"BlockPrefabs/Empty");
    }
}

public class BlockGameObject
{
    public readonly BlockDirections EntranceDirection;
    public readonly BlockDirectionsNumbers EntranceNumber;
    public readonly BlockDirections ExitDirection;
    public readonly BlockDirectionsNumbers ExitNumber;
    public readonly GameObject Prefab;

    public BlockGameObject(GameObject prefab)
    {
        var prefabName = prefab.name;
        (EntranceDirection, EntranceNumber, ExitDirection, ExitNumber) = ParsePrefabName(prefabName);
        Prefab = prefab;
    }

    public BlockGameObject(BlockDirections entranceDirection,
        BlockDirectionsNumbers entranceNumber,
        BlockDirections exitDirection,
        BlockDirectionsNumbers exitNumber,
        GameObject prefab)
    {
        EntranceDirection = entranceDirection;
        EntranceNumber = entranceNumber;
        ExitDirection = exitDirection;
        ExitNumber = exitNumber;
        Prefab = prefab;
    }

    private (BlockDirections, BlockDirectionsNumbers, BlockDirections, BlockDirectionsNumbers) ParsePrefabName(
        string prefabName)
    {
        var m = prefabName.Split('_');
        return (GetBlockDirectionByStr(m[0]), GetBlockDirectionNumberByStr(m[1]),
            GetBlockDirectionByStr(m[2]), GetBlockDirectionNumberByStr(m[3]));
    }

    private static BlockDirections GetBlockDirectionByStr(string strDirection)
    {
        return strDirection switch
        {
            "Down" => BlockDirections.Down,
            "Right" => BlockDirections.Right,
            "Up" => BlockDirections.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(strDirection), strDirection, null)
        };
    }

    private static BlockDirectionsNumbers GetBlockDirectionNumberByStr(string strDirectionNumber)
    {
        return strDirectionNumber switch
        {
            "1" => BlockDirectionsNumbers.First,
            "2" => BlockDirectionsNumbers.Second,
            "3" => BlockDirectionsNumbers.Third,
            _ => throw new ArgumentOutOfRangeException(nameof(strDirectionNumber), strDirectionNumber, null)
        };
    }
}

public enum BlockDirections
{
    Up,
    Right,
    Down
}

public enum BlockDirectionsNumbers
{
    First,
    Second,
    Third
}