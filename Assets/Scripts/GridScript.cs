using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GridScript : MonoBehaviour
{
    public BlockScript playerBlockScript;
    public BlockScript firstBlockScript;
    public BlockScript lastExistingBlockScript;
    private readonly Random random = new();
    public string pathToBlocks;

    private const int BlockWidth = 64;
    private const int BlockHeight = 48;

    private BlockGameObject startBlock;
    private BlockGameObject endBlock;
    private bool stopGeneration;

    private readonly Dictionary<BlockDirections, Dictionary<BlockDirectionsNumbers, List<BlockGameObject>>> directions =
        new()
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

    public void InitGrid(string pathToBlocks0)
    {
        pathToBlocks = pathToBlocks0;
        LoadBlockPrefabs();
        InitStartBlock();
        GenerateNextLevelBlock();
    }

    private void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            ChangeIfNeedPlayerBlock();
            if (!stopGeneration
                && (playerBlockScript.exitBlockScript == lastExistingBlockScript
                    || playerBlockScript.exitBlockScript.exitBlockScript == lastExistingBlockScript))
                GenerateNextLevelBlock();

            yield return new WaitForFixedUpdate();
        }
    }

    private void ChangeIfNeedPlayerBlock()
    {
        var playerPosition = CurrentGame.Player.transform.position;
        var playerBlockPosition = playerBlockScript.transform.position;
        if (playerPosition.x > playerBlockPosition.x + BlockWidth
            && playerBlockScript.exitDirection == BlockDirections.Right)
            playerBlockScript = playerBlockScript.exitBlockScript;
        if (playerPosition.x < playerBlockPosition.x - BlockWidth
            && playerBlockScript.entranceDirection == BlockDirections.Right)
            playerBlockScript = playerBlockScript.entranceBlockScript;
        if (playerPosition.y > playerBlockPosition.y + BlockHeight)
            if (playerBlockScript.exitDirection == BlockDirections.Up)
                playerBlockScript = playerBlockScript.exitBlockScript;
            else if (playerBlockScript.entranceDirection == BlockDirections.Up)
                playerBlockScript = playerBlockScript.entranceBlockScript;
        if (playerPosition.y < playerBlockPosition.y - BlockHeight)
            if (playerBlockScript.exitDirection == BlockDirections.Down)
                playerBlockScript = playerBlockScript.exitBlockScript;
            else if (playerBlockScript.entranceDirection == BlockDirections.Down)
                playerBlockScript = playerBlockScript.entranceBlockScript;
    }

    private void InitStartBlock()
    {
        // var prefabList = Directions[BlockDirections.Right][BlockDirectionsNumbers.First];
        if (startBlock == null)
        {
            Debug.Log($"Не могу создать начальный блок");
            return;
        }

        var blockScript = Instantiate(startBlock.Prefab, transform, true).GetComponent<BlockScript>();
        blockScript.Init(this, Vector3.zero, null, BlockDirections.Right, BlockDirectionsNumbers.First,
            startBlock.ExitDirection, startBlock.ExitNumber);
        playerBlockScript = blockScript;
        firstBlockScript = blockScript;
        lastExistingBlockScript = blockScript;
        Debug.Log("Начальный блок сгенерирован");
    }

    private void GenerateNextLevelBlock()
    {
        if (lastExistingBlockScript.exitDirection == BlockDirections.End)
        {
            stopGeneration = true;
            return;
        }

        var prefabList = directions[lastExistingBlockScript.exitDirection][lastExistingBlockScript.exitNumber];
        var nextBlockPrefab = prefabList.Count == 0
            ? endBlock
            : prefabList[random.Next(prefabList.Count)];
        if (nextBlockPrefab == endBlock)
            stopGeneration = true;

        var nextBlockPosition = lastExistingBlockScript.transform.position + GetDirectionToNextBlock();
        var nextBlockScript = Instantiate(nextBlockPrefab.Prefab, transform, true).GetComponent<BlockScript>();
        nextBlockScript.Init(this, nextBlockPosition, lastExistingBlockScript,
            lastExistingBlockScript.entranceDirection,
            lastExistingBlockScript.entranceNumber,
            nextBlockPrefab.ExitDirection, nextBlockPrefab.ExitNumber);

        lastExistingBlockScript.exitBlockScript = nextBlockScript;
        lastExistingBlockScript = nextBlockScript;
    }

    private Vector3 GetDirectionToNextBlock()
    {
        var x = lastExistingBlockScript.exitDirection == BlockDirections.Right ? BlockWidth : 0;
        var y = lastExistingBlockScript.exitDirection switch
        {
            BlockDirections.Up => BlockHeight,
            BlockDirections.Down => -BlockHeight,
            _ => 0
        };
        return new Vector3(x, y);
    }


    private void LoadBlockPrefabs()
    {
        var prefabs = Resources.LoadAll<GameObject>($"Levels/{pathToBlocks}");
        Debug.Log($"Из Levels/{pathToBlocks} загружено {prefabs.Length} блоков");
        foreach (var prefab in prefabs)
        {
            var block = new BlockGameObject(prefab);
            if (block.EntranceDirection == BlockDirections.Start)
                startBlock = block;
            else if (block.EntranceDirection == BlockDirections.End)
                endBlock = block;
            else if (block.Number != 0 && block.EntranceDirection != BlockDirections.Base)
                directions[block.EntranceDirection][block.EntranceNumber].Add(block);
        }
    }
}