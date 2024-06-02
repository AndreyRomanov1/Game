using System;
using System.Collections;
using System.Linq;
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

    private BaseGunScript gun;
    private PlayerScript player;
    private SpriteRenderer healthIndicator;

    private bool isPlayerWasSaw; 

    private void Start()
    {
        gun = Instantiate(gunPrefab, gunPosition.transform).GetComponent<BaseGunScript>();
        player = CurrentGame.Player;
        healthIndicator = transform.Find("нимб").GetComponent<SpriteRenderer>();
        healthPoints = MaxHealthPoints;

        // StartCoroutine(PlayerSearch());
        StartCoroutine(GuidingHandsCoroutine());
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator GuidingHandsCoroutine()
    {
        // yield return new WaitForSeconds(DelayBeforeFiring); // Я убрал, вроде стало получше, если будет слишком жёстко, вернём
        // Debug.Log("Start shoot");

        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!IsPlayerInSight(out var hit))
                continue;
            
            if (Model.GameState != GameStates.ActiveGame)
                yield return null;

            if (!isPlayerWasSaw)
            {
                Debug.Log("Впервые увидел игрока");
                isPlayerWasSaw = true;
                TimeController.EnemyHasDetectedPlayerHandler();
            }
            
            var hitLine = hit.point - (Vector2)pivot.transform.position;
            var currentVectorRotation = pivot.transform.rotation.eulerAngles.z;
            var expectedVectorRotation = Mathf.Atan2(hitLine.y, hitLine.x) * Mathf.Rad2Deg + 180;

            var shootingError = Math.Abs(expectedVectorRotation - currentVectorRotation) < 180 
                ? expectedVectorRotation - currentVectorRotation 
                : currentVectorRotation - expectedVectorRotation;

            // Debug.Log($"{shootingError}:  {expectedVectorRotation}   -   {currentVectorRotation}");

            if (Math.Abs(shootingError) > PermissibleShootingError)
            {
                var rotation = shootingError > PermissibleShootingError ? HandSwingSpeed : -HandSwingSpeed;
                pivot.transform.rotation = Quaternion.Euler(0f, 0f, currentVectorRotation + rotation);
                // Debug.Log($"{pivot.transform.rotation}: {currentVectorRotation} + {rotation}");
            }
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            var hitLine = Quaternion.Euler(0, 0, pivot.transform.rotation.eulerAngles.z) * new Vector3(-1, 0);
            // Debug.Log($"{hitLine} {(gunPosition.transform.position - pivot.transform.position).normalized}");
            if (IsPlayerOnLine(pivot.transform.position, hitLine.normalized))
                Shoot();
            yield return new WaitForFixedUpdate();
        }
    }

    private bool IsPlayerInSight() => IsPlayerInSight(out _);
    private bool IsPlayerInSight(out RaycastHit2D hit)
    {
        hit = default;
        var startPoint = pivot.transform.position;
        foreach (var vector in player.Targets.Select(target => (target.position - startPoint).normalized))
        {
            if (IsPlayerOnLine(startPoint, vector, out hit))
                return true;
        }

        // Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
        return false;
    }

    private bool IsPlayerOnLine(Vector2 startPoint, Vector2 direction) => IsPlayerOnLine(startPoint, direction, out _);
    private bool IsPlayerOnLine(Vector2 startPoint, Vector2 direction, out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(startPoint, direction, ShootingDistance, detectedLayers);
        var isPlayerSeeEnemy = Tools.IsPointInPlayerCamera(transform.position);
        return hit.transform is not null && hit.transform.gameObject == player.gameObject && isPlayerSeeEnemy;
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
        var colorValue = healthPoints / MaxHealthPoints;
        healthIndicator.color = new Color(1f, colorValue, colorValue);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}