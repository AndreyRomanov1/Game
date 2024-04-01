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
    
    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            Shoot();
    }

    private void Shoot()
    {
        var currentBullet = Instantiate(bullet).GetComponent<IShooted>();
        currentBullet.Shoot(transform, bulletSpeed, gameController);
    }
}
