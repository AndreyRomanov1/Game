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
    private LayerMask mask;

    
    public void Shoot(UnityEngine.Vector3 position, Quaternion rotation, float speed, GameController gameController, LayerMask mask)
    {
        var transformThis = transform;
        transformThis.position = position;
        transformThis.rotation = rotation;
        this.speed = speed;
        this.gameController = gameController;
        this.mask = mask;
    }
    
    //TODO: можно переписать на корутину
    void Update()
    {
        var lastPos = transform.position;
        var displacement = transform.right * (gameController.GameSpeed * speed * Time.deltaTime);
        transform.Translate(displacement);

        if (FindObjectOnLine(lastPos, transform.position, out var hitted))
        {
            CollisionLogic(hitted);
        }
    }

    void CollisionLogic(GameObject other)
    {
        Destroy(this.GameObject());
    }
    
    //TODO: можно переписать на расширения класса Physics2D
    bool FindObjectOnLine(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 endPosition, out GameObject result)
    {
        result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
        return result is not null;
    }
}

