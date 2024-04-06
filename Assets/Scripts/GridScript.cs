using System.Collections.Generic;
using Model;
using UnityEngine;
using Random = System.Random;

public class GridScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public const int BlockWidth = 64;
    public const int BlockHeight = 48;
    public Player player;
    public Block PlayerBlock;
    public Block FirstBlock;
    public Block LastExistingBlock;
    private readonly Random random = new();

    private readonly Dictionary<BlockDirections, Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>> Directions = new()
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

    private void Start()
    {
        LoadBlockPrefabs();
        InitStartBlock();
        GenerateNextLevelBlock();
        InitPlayer();
    }

    private void Update()
    {
        ChangeIfNeedPlayerBlock();
        if (PlayerBlock.exitBlock == LastExistingBlock || PlayerBlock.exitBlock.exitBlock == LastExistingBlock)
            GenerateNextLevelBlock();
    }

    private void ChangeIfNeedPlayerBlock()
    {
        var playerPos = player.transform.position;
        var playerBlockPos = PlayerBlock.transform.position;
        if (playerPos.x > playerBlockPos.x + BlockWidth
            && PlayerBlock.exitDirection == BlockDirections.Right)
            PlayerBlock = PlayerBlock.exitBlock;
        if (playerPos.x < playerBlockPos.x - BlockWidth
            && PlayerBlock.entranceDirection == BlockDirections.Right)
            PlayerBlock = PlayerBlock.entranceBlock;
        if (playerPos.y > playerBlockPos.y + BlockHeight)
            if (PlayerBlock.exitDirection == BlockDirections.Up)
                PlayerBlock = PlayerBlock.exitBlock;
            else if (PlayerBlock.entranceDirection == BlockDirections.Up)
                PlayerBlock = PlayerBlock.entranceBlock;
        if (playerPos.y < playerBlockPos.y - BlockHeight)
            if (PlayerBlock.exitDirection == BlockDirections.Down)
                PlayerBlock = PlayerBlock.exitBlock;
            else if (PlayerBlock.entranceDirection == BlockDirections.Down)
                PlayerBlock = PlayerBlock.entranceBlock;
    }

    private void InitStartBlock()
    {
        var prefabList = Directions[BlockDirections.Right][BlockDirectionsNumbers.First];
        if (prefabList.Count == 0)
        {
            Debug.Log($"Не могу создать начальный блок");
            return;
        }

        var prefab = prefabList[random.Next(prefabList.Count)];
        var blockScript = Instantiate(prefab.Prefab, transform, true).GetComponent<Block>();
        blockScript.Init(this, Vector3.zero, null, BlockDirections.Right, BlockDirectionsNumbers.First,
            prefab.ExitDirection, prefab.ExitNumber);
        PlayerBlock = blockScript;
        FirstBlock = blockScript;
        LastExistingBlock = blockScript;
    }

    private void GenerateNextLevelBlock()
    {
        var prefabList = Directions[LastExistingBlock.exitDirection][LastExistingBlock.exitNumber];
        if (prefabList.Count == 0)
        {
            Debug.Log($"Нет подходящих блоков: {LastExistingBlock.exitDirection} {LastExistingBlock.exitNumber}");
            return;
        }

        var nextBlockPosition = LastExistingBlock.transform.position + GetDirectionToNextBlock();
        var nextBlockPrefab = prefabList[random.Next(prefabList.Count)];
        var nextBlockScript = Instantiate(nextBlockPrefab.Prefab, transform, true).GetComponent<Block>();
        nextBlockScript.Init(this, nextBlockPosition, LastExistingBlock, LastExistingBlock.entranceDirection,
            LastExistingBlock.entranceNumber,
            nextBlockPrefab.ExitDirection, nextBlockPrefab.ExitNumber);

        LastExistingBlock.exitBlock = nextBlockScript;
        LastExistingBlock = nextBlockScript;
    }

    private Vector3 GetDirectionToNextBlock()
    {
        var x = LastExistingBlock.exitDirection == BlockDirections.Right ? BlockWidth : 0;
        var y = LastExistingBlock.exitDirection == BlockDirections.Up
            ? BlockHeight
            : LastExistingBlock.exitDirection == BlockDirections.Down
                ? -BlockHeight
                : 0;
        return new Vector3(x, y);
    }


    private void InitPlayer()
    {
        player = Instantiate(playerPrefab).GetComponent<Player>();
    }

    private void LoadBlockPrefabs()
    {
        var paths = new[]
        {
            "DownEntrance/1", //"DownEntrance/2", "DownEntrance/3",
            "UpEntrance/1", //"UpEntrance/2", "UpEntrance/3",
            "RightEntrance/1", //"RightEntrance/2"
        };
        foreach (var path in paths)
        {
            var prefabs = Resources.LoadAll<GameObject>($"BlockPrefabs/{path}");
            foreach (var prefab in prefabs)
            {
                var g = new BlockGameObject(prefab);
                if (g.Number != 0)
                    Directions[g.EntranceDirection][g.EntranceNumber].Add(g);
            }
        }

        // emptyPrefabs = Resources.LoadAll<GameObject>($"BlockPrefabs/Empty");
    }
}