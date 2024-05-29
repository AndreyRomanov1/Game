using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class RocketScript: BaseProjectileScript
{
    private Object explosion;
    private LayerMask explosionMask;
    private float explosionRadius;
    private WeaponStateEnum weaponState;
    
    private void Start()
    {
        explosion = Resources.Load("Other Elements/Exposion");
    }
    
    public void Shoot(Transform transformParent, float speed0, float lifetime,
        LayerMask mask0, LayerMask expMask,
        float weaponDamage = 50f, float expRadius = 15f, WeaponStateEnum state = WeaponStateEnum.Nothing)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        _speed = speed0;
        _lifetime = lifetime;
        _mask = mask0;
        damage = weaponDamage;

        explosionMask = expMask;
        explosionRadius = expRadius;
        weaponState = state;

        StartCoroutine(FixedUpdateCoroutine());
    }

    protected override void CollisionLogic(GameObject other)
    {
        // Debug.Log($"Колизия: {other.name}");
        // var colliders = GetComponents<CircleCollider2D>();
        // var movementCollider = colliders.First(x => x.radius <= 1);
        // var explosionCollider = colliders.First(x => x.radius >= 5);
        
        // movementCollider.enabled = false;
        // explosionCollider.enabled = true;
        
        Instantiate(explosion).GetComponent<ExplosionScript>().Init(transform.position, damage, explosionRadius, 
            explosionMask, weaponState);
        Destroy(gameObject);
    }
    
    
}