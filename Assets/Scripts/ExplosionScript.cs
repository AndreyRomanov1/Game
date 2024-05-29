using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionScript: MonoBehaviour
{
    public LayerMask explosionMask;
    
    private CircleCollider2D collider0;
    private float explosionDamage;
    private WeaponStateEnum explosionInitiator;

    public void Init(Vector2 position, float damage, float radius = 15f, LayerMask layerMask = default,
        WeaponStateEnum initiator = WeaponStateEnum.Nothing)
    {
        transform.position = position;
        explosionDamage = damage;
        collider0 = GetComponent<CircleCollider2D>();
        if (layerMask != default)
            explosionMask = layerMask;
        collider0.radius = radius;
        collider0.contactCaptureLayers = explosionMask;
        collider0.callbackLayers = explosionMask;
        explosionInitiator = initiator;
        StartCoroutine(LetDestroy());

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Задел объект: {other.name}");
        var damage = explosionDamage;
        if (other.gameObject.layer ==WeaponState.StateToLayerConnectedObject(explosionInitiator))
            damage /= 2;
        other.GetComponent<IDamageable>().TakeDamage(damage);
    }

    private IEnumerator LetDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}

