using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Pistol : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 20;
    public float rateOfFire = 1;
    public LayerMask mask;
    
    private GameController gameController;
    private float timeBetweenShots;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        timeBetweenShots = 1 / rateOfFire;

        StartCoroutine(CheckShot());
    }

    IEnumerator CheckShot()
    {
        while (true)
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenShots);
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
    
    // void Update()
    // {
    //     if (Input.GetMouseButton((int)MouseButton.LeftMouse))
    //         Shoot();
    // }

    private void Shoot()
    {
        var currentBullet = Instantiate(bullet).GetComponent<IShooted>();
        currentBullet.Shoot(transform, bulletSpeed, gameController, mask);
    }
}
