using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Bullet : MonoBehaviour, IShooted
{
    public LayerMask mask;
    
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
    
    void Update()
    {
        var transformThis = transform;
        var displacement = transformThis.right * (gameController.GameSpeed * speed * Time.deltaTime);
        var newPos = transformThis.position + displacement;

        // Debug.Log(Physics2D.Linecast(transformThis.position, newPos).transform.GameObject());
        if (Linecast2D(transformThis.position, newPos, out var hitted))
            transform.Translate(displacement);
        else
        {
            // Debug.Log("Linecast");
            transform.Translate(displacement);
            CollisionLogic(hitted);
        }
    }

    void CollisionLogic(GameObject other)
    {
        Destroy(this.GameObject());
    }
    
    //TODO: можно переписать на расширения класса Physics2D
    bool Linecast2D(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 endPosition, out GameObject result)
    {
        result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
        return result is null;
    }
}

