using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketScript: BaseProjectileScript
{
    
    protected override void CollisionLogic(GameObject other)
    {
        // Debug.Log($"Колизия: {other.name}");
        var colliders = GetComponents<CircleCollider2D>();
        var movementCollider = colliders.First(x => x.radius <= 1);
        var explosionCollider = colliders.First(x => x.radius >= 5);
        
        movementCollider.enabled = false;
        explosionCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");

        other.GetComponent<IDamageable>().TakeDamage(damage);
        StartCoroutine(WaitNext());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stay");
    }

    private IEnumerator WaitNext()
    {
        yield return null;
        Destroy();
    }
    
    
}