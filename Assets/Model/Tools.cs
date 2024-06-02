using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public static class Tools
{
    public static bool FindObjectOnLine(Vector2 startPosition, Vector2 endPosition, int mask, out GameObject result)
    {
        result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
        return result is not null;
    }

    public static GameObject SpawnEnemy(Vector2 position, GameObject parent = null)
    {
        var random = new Random();
        var enemies = Resources
            .LoadAll<GameObject>("Enemies")
            .Where(obj => obj.layer == LayerMask.NameToLayer("Enemies"))
            .ToArray();
        var chosenEnemy = enemies[random.Next(enemies.Length)];
        return SpawnObject(chosenEnemy, position, parent);
    }

    public static GameObject SpawnCollectionObject(Vector2 position, GameObject parent = null)
    {
        var collectionObject = Resources.Load<GameObject>("Other Elements/CollectionTrigger");
        var obj = SpawnObject(collectionObject, position, parent);
        obj.GetComponent<CollectionTriggerScript>().CreateRandomTrigger(position);
        return obj;
    }

    public static GameObject SpawnObject(GameObject prefab, Vector2 position, GameObject parent = null)
    {
        var obj = parent is not null
            ? Object.Instantiate(prefab, parent.transform, true)
            : GameScript.CreateByGameObjectInCurrentGame(prefab);
        obj.SetActive(true);
        obj.transform.position = position;
        return obj;
    }

    public static bool IsPointInPlayerCamera(Vector2 point)
    {
        var cameraRect = CurrentGame.PlayerCamera.pixelRect;
        return point.x >= cameraRect.x && point.x <= cameraRect.x + cameraRect.width
            &&  point.y >= cameraRect.y && point.y <= cameraRect.y + cameraRect.height;
    }
}