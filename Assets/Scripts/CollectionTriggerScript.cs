using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;


public class CollectionTriggerScript : MonoBehaviour
{
    private static readonly Dictionary<string, int> foldersChance = new()
    {
        { "Weapons/Guns",  3}, { "Heals", 7 }
    };

    private Random random;
    private PlayerScript player;
    private GameObject ContainedObject;
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
        Debug.Log($"{ContainedObject?.name} -> {contained?.name}");
        ContainedObject = contained;
        transform.position = contained.transform.position;
        ContainedObject.gameObject.transform.parent = transform;
        ContainedObject.transform.localPosition = new Vector3(0, 0, 0);
        return gameObject;
    }

    public GameObject CreateTrigger(Vector3 position)
    {
        CreateRandom();
        var folder = random.ProbabilisticRandom(foldersChance);
        var elements = Resources.LoadAll<GameObject>(folder);
        // Debug.Log($"{string.Join(" ", guns.Select(x => x.name).ToArray())}");
        var num = random.Next(elements.Length);
        Debug.Log(num);
        var gun = Instantiate(elements[num]);
        gun.transform.position = position;
        return CreateTrigger(gun);
    }

    IEnumerator WaitAnotherAction()
    {
        yield return null;
        if (ContainedObject is null)
            CreateTrigger(new Vector3(0,0));
    }

    IEnumerator CheckInteraction()
    {
        yield return null;
        while (true)
        {
            // Debug.Log(distanceToPlayer);
            if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
            {
                // Debug.Log("Нажал подобрать");
                ContainedObject.GetComponent<IPickable>().PickUp(player);
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
        player.ShowButtonIcon(Buttons.F);
        if (other.CompareTag("Player"))
            isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        player.HideButtonIcon(Buttons.F);
        if (other.CompareTag("Player"))
            isPlayerInTrigger = false;   
    }
}
