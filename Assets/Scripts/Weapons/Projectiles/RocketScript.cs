using UnityEngine;

public class RocketScript : BaseProjectileScript
{
    private GameObject explosion;
    private float explosionRadius;
    private WeaponStateEnum weaponState;

    private void Start()
    {
        explosion = Resources.Load<GameObject>("Other Elements/Exposion");
    }

    public void Shoot(Transform transformParent, float speed0, float lifetime0,
        LayerMask mask0,
        float weaponDamage = 50f, float expRadius = 15f, WeaponStateEnum state = WeaponStateEnum.Nothing)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        speed = speed0;
        lifetime = lifetime0;
        mask = mask0;
        damage = weaponDamage;

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

        GameScript.CreateByGameObjectInCurrentGame(explosion)
            .GetComponent<ExplosionScript>()
            .Init(transform.position, damage, explosionRadius, weaponState);
        Destroy(gameObject);
    }
}