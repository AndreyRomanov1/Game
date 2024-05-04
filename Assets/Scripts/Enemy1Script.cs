using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    public GameObject pivot;
    public GameObject gunPosition;
    public GameObject gunPrefab;

    private GameObject gun;
    private GameObject player;

    private readonly float shootingDistance = 15f;
    private readonly float delayBeforeFiring = 2f;
    private readonly float handSwingSpeed = 1f;
    private readonly float permissibleShootingError = 5f;

    // Start is called before the first frame update
    void Start()
    {
        gun = Instantiate(gunPrefab, gunPosition.transform);
        player = CurrentGame.Player.gameObject;
        
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
        }

        StartCoroutine(StartShooting());
    }
    
    private IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(delayBeforeFiring);

        while (IsPlayerOnLine(out var hit))
        {
            var currentVectorRotation = transform.rotation.z;
            var expectedVectorRotation = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
            var shootingError = expectedVectorRotation - currentVectorRotation;
            
            if (Math.Abs(shootingError) <= permissibleShootingError)
                Shoot();
            else
            {
                var rotation = shootingError > permissibleShootingError ? handSwingSpeed : - handSwingSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, currentVectorRotation + rotation);
            }

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(PlayerSearch());
    }

    private bool IsPlayerOnLine()
    { 
        var hit = Physics2D.Linecast(gunPosition.transform.position, player.transform.position);
        return hit.transform.gameObject == player && hit.distance <= shootingDistance;
    }
    
    private bool IsPlayerOnLine(out RaycastHit2D hit)
    { 
        hit = Physics2D.Linecast(gunPosition.transform.position, player.transform.position);
        return hit.transform.gameObject == player && hit.distance <= shootingDistance;
    }

    private void Shoot()
    {}
}
