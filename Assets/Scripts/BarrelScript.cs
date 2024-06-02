using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class BarrelScript : MonoBehaviour, IDamageable
{
    private float HP;
    private float maxHP = 40;

    private SpriteRenderer spriteRenderer;

    private readonly Dictionary<Func<Vector2, GameObject, GameObject>, int> spawnRate = new()
        {
            { Tools.SpawnCollectionObject, 4 },
            { Tools.SpawnEnemy, 1 },
            { (_, _) => null, 5 }
        };

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
            DestroyObject();
        var colorValue = (HP / maxHP);
        spriteRenderer.color = new Color(1f, colorValue, colorValue);
    }

    private void DestroyObject()
    {
        if (!Model.IsEducation)
        {
            var random = new Random();
            var func = random.ProbabilisticRandom(spawnRate);
            func(transform.position, null);
        }
        
        Destroy(gameObject);
    }
}
