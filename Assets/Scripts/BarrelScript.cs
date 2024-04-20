using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelScript : MonoBehaviour, IDamageable
{
    public int cost = 0;
    
    private float HP;
    private float maxHP = 100;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
            Destroy(this.GameObject());
        var colorValue = (HP / maxHP);
        spriteRenderer.color = new Color(1f, colorValue, colorValue);
    }
}
