using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class CollectionTriggerScript : MonoBehaviour
{
    private static readonly Dictionary<string, int> foldersChance = new()
    {
        { "Weapons/Guns", 2 }, { "Heals", 8 }
    };

    private Random random;
    private PlayerScript player;
    private GameObject containedObject;
    private bool isPlayerInTrigger = false;

    public void Start()
    {
        player = CurrentGame.Player;
        CreateRandom();
        StartCoroutine(WaitAnotherAction());
        StartCoroutine(CheckInteraction());
    }

    public GameObject CreateTrigger(GameObject contained)
    {
        CreateRandom();
        if (contained is null)
            Destroy(gameObject);
        // Debug.Log($"{containedObject?.name} -> {contained?.name}");
        containedObject = contained;
        transform.position = contained.transform.position;
        containedObject.gameObject.transform.parent = transform;
        Debug.Log($"{containedObject.gameObject.transform.parent.name}");

        containedObject.transform.localPosition = new Vector3(0, 0, 0);
        return gameObject;
    }
    
    public GameObject CreateTrigger(GameObject contained, Vector3 position)
    {
        CreateRandom();
        if (contained is null)
            Destroy(gameObject);
        // Debug.Log($"{containedObject?.name} -> {contained?.name}");
        containedObject = contained;
        transform.position = position;
        // Debug.Log($"2  {containedObject}, {containedObject.gameObject}, {containedObject.gameObject.transform}, {containedObject.gameObject.transform.parent}:  {transform}");
        containedObject.gameObject.transform.parent = transform;
        containedObject.transform.localPosition = new Vector3(0, 0, 0);
        return gameObject;
    }

    public GameObject CreateRandomTrigger(Vector3 position)
    {
        CreateRandom();
        var folder = random.ProbabilisticRandom(foldersChance);
        var elements = Resources.LoadAll<GameObject>(folder);
        // Debug.Log($"{string.Join(" ", guns.Select(x => x.name).ToArray())}");
        var num = random.Next(elements.Length);
        Debug.Log(num);
        var gun = GameScript.CreateByGameObjectInCurrentGame(elements[num]);
        gun.transform.position = position;
        return CreateTrigger(gun);
    }

    private IEnumerator WaitAnotherAction()
    {
        yield return null;
        if (containedObject is null)
            CreateRandomTrigger(new Vector3(0, 0));
    }

    private IEnumerator CheckInteraction()
    {
        yield return null;
        while (true)
        {
            // Debug.Log(distanceToPlayer);
            if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
            {
                // Debug.Log("Нажал подобрать");
                containedObject.GetComponent<IPickable>().PickUp(player);
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void CreateRandom() =>
        random ??= new Random();


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Contact object: {other.tag}");
        player.ShowButtonIcon(ButtonsEnum.F);
        if (other.CompareTag("Player"))
            isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        player.HideButtonIcon(ButtonsEnum.F);
        if (other.CompareTag("Player"))
            isPlayerInTrigger = false;
    }
}