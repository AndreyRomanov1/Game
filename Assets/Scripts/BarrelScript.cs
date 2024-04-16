using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelScript : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        Destroy(this.GameObject());
    }
}
