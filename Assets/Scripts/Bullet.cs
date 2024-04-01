using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Bullet : MonoBehaviour, IShooted
{
    private float speed;
    private GameController gameController;
    
    public void Shoot(UnityEngine.Vector3 position, Quaternion rotation, float speed, GameController gameController)
    {
        var transformThis = transform;
        transformThis.position = position;
        transformThis.rotation = rotation;
        this.speed = speed;
        this.gameController = gameController;
    }
    
    void FixedUpdate()
    {
        Debug.Log($"Bullet {gameController.GameSpeed}");
        transform.Translate(transform.right * (gameController.GameSpeed * speed * Time.deltaTime));
    }
}

