using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionScript: MonoBehaviour
{
    public LayerMask ExplosionMask;
    
    private CircleCollider2D collider;

    public void Init(Vector2 position, float radius = 15f, LayerMask layerMask = default)
    {
        transform.position = position;
        collider = GetComponent<CircleCollider2D>();
        if (layerMask != default)
            ExplosionMask = layerMask;
        collider.radius = radius;
        collider.contactCaptureLayers = ExplosionMask;
        collider.callbackLayers = ExplosionMask;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stay");
    }
}

