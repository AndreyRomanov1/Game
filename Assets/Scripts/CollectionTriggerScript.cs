using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;


public class CollectionTriggerScript : MonoBehaviour
{
    private static readonly string gunFolder = "Weapons/Guns";

    private PlayerScript player;
    private GameObject ContainedObject;
    private bool isPlayerInTrigger = false;

    private void Start()
    {
        player = CurrentGame.Player;
        StartCoroutine(WaitAnotherAction());
        StartCoroutine(CheckInteraction());
    }

    public GameObject CreateTrigger(GameObject contained)
    {
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
        var guns = Resources.LoadAll<GameObject>(gunFolder);
        var rand = new Random();
        // Debug.Log($"{string.Join(" ", guns.Select(x => x.name).ToArray())}");
        var num = rand.Next(guns.Length);
        Debug.Log(num);
        var gun = Instantiate(guns[num]);
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Contact object: {other.tag}");
        if (other.CompareTag("Player"))
            isPlayerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInTrigger = false;   
    }
}
