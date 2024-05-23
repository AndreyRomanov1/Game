using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class CollectionTriggerScript : MonoBehaviour
{
    private static string gunFolder = "Weapons/Guns";
    
    private GameObject ContainedObject;

    private void Start()
    {
        CreateTrigger(new Vector3(0,0));
    }

    public GameObject CreateTrigger(GameObject contained)
    {
        ContainedObject = contained;
        transform.position = contained.transform.position;
        ContainedObject.gameObject.transform.parent = transform;
        ContainedObject.transform.position = new Vector3(0, 0, 0);
        return gameObject;
    }

    public GameObject CreateTrigger(Vector3 position)
    {
        var guns = Resources.LoadAll<GameObject>(gunFolder);
        var rand = new Random();
        // Debug.Log($"{string.Join(" ", guns.Select(x => x.name).ToArray())}");
        var gun = Instantiate(guns[rand.Next(guns.Length - 1)]);
        gun.transform.position = position;
        return CreateTrigger(gun);
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            ContainedObject.GetComponent<IPickable>().PickUp(other.GetComponent<PlayerScript>());
            Destroy(gameObject);
        }
    }
}
