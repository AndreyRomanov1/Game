using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionScript: MonoBehaviour
{
    public LayerMask ExplosionMask;
    
    private CircleCollider2D collider;
    private float explosionDamage;
    private WeaponStateEnum explosionInitiator;

    public void Init(Vector2 position, float damage, float radius = 15f, LayerMask layerMask = default,
        WeaponStateEnum initiator = WeaponStateEnum.Nothing)
    {
        transform.position = position;
        explosionDamage = damage;
        collider = GetComponent<CircleCollider2D>();
        if (layerMask != default)
            ExplosionMask = layerMask;
        collider.radius = radius;
        collider.contactCaptureLayers = ExplosionMask;
        collider.callbackLayers = ExplosionMask;
        explosionInitiator = initiator;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Задел объект: {other.name}");
        var damage = explosionDamage;
        if (other.gameObject.layer ==WeaponState.StateToLayerConnectedObject(explosionInitiator))
            damage /= 2;
        other.GetComponent<IDamageable>().TakeDamage(damage);
        StartCoroutine(WaitNextUpdateCoroutine());
    }

    private IEnumerator WaitNextUpdateCoroutine()
    {
        yield return null;
        Destroy(gameObject);
    }
}

