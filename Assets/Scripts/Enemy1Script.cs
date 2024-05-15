using System;
using System.Collections;
using UnityEngine;

public class Enemy1Script : MonoBehaviour, IDamageable
{
    private const float ShootingDistance = 15f;
    private const float DelayBeforeFiring = 2f;
    private const float HandSwingSpeed = 1f;
    private const float PermissibleShootingError = 1f;
    private const float MaxHealthPoints = 50;
    private float healthPoints;

    public GameObject pivot;
    public GameObject gunPosition;
    public GameObject gunPrefab;
    public LayerMask detectedLayers;

    private PistolScript gun;
    private GameObject player;
    private SpriteRenderer healthIndicator;

    void Start()
    {
        gun = Instantiate(gunPrefab, gunPosition.transform).GetComponent<PistolScript>();
        player = CurrentGame.Player.gameObject;
        healthIndicator = transform.Find("нимб").GetComponent<SpriteRenderer>();
        healthPoints = MaxHealthPoints;

        StartCoroutine(PlayerSearch());
    }

    private IEnumerator PlayerSearch()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (IsPlayerOnLine())
                break;
            yield return new WaitForFixedUpdate();
            // Debug.Log("Не на линии");
        }

        StartCoroutine(StartShooting());
    }

    private IEnumerator StartShooting()
    {
        // yield return new WaitForSeconds(DelayBeforeFiring); // Я убрал, вроде стало получше, если будет слишком жёстко, вернём
        Debug.Log("Start shoot");

        while (IsPlayerOnLine(out var hit))
        {
            var hitLine = hit.transform.position - pivot.transform.position;
            var currentVectorRotation = pivot.transform.rotation.eulerAngles.z;
            var expectedVectorRotation = Mathf.Atan2(hitLine.y, hitLine.x) * Mathf.Rad2Deg + 180;

            var shootingError = expectedVectorRotation - currentVectorRotation;
            Debug.Log($"{shootingError}:  {expectedVectorRotation}   -   {currentVectorRotation}");

            if (Math.Abs(shootingError) <= PermissibleShootingError)
                Shoot();
            else
            {
                var rotation = shootingError > PermissibleShootingError ? HandSwingSpeed : -HandSwingSpeed;
                pivot.transform.rotation = Quaternion.Euler(0f, 0f, currentVectorRotation + rotation);
                // pivot.transform.rotation.Set(0f, 0f, currentVectorRotation + rotation, 0f);
                // Debug.Log($"{pivot.transform.rotation}: {currentVectorRotation} + {rotation}");
            }

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(PlayerSearch());
    }

    private bool IsPlayerOnLine() => IsPlayerOnLine(out _);

    private bool IsPlayerOnLine(out RaycastHit2D hit)
    {
        var startPoint = pivot.transform.position;
        var vector = (player.transform.position - startPoint).normalized;
        hit = Physics2D.Raycast(startPoint, vector, ShootingDistance, detectedLayers);
        // Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
        return hit.transform is not null && hit.transform.gameObject == player;
    }

    private void Shoot()
    {
        gun.ShootSignal();
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
            Die();
        var colorValue = (healthPoints / MaxHealthPoints);
        healthIndicator.color = new Color(1f, colorValue, colorValue);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}