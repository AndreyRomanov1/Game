using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Pistol : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 20;
    public float rateOfFire = 1;
    public LayerMask mask;

    private float timeBetweenShots;

    private void Start()
    {
        timeBetweenShots = 1 / rateOfFire;

        StartCoroutine(CheckShot());
    }

    private IEnumerator CheckShot()
    {
        while (true)
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenShots);
            }
            else
                yield return new WaitForFixedUpdate();
        }
    }

    // void Update()
    // {
    //     if (Input.GetMouseButton((int)MouseButton.LeftMouse))
    //         Shoot();
    // }

    private void Shoot()
    {
        var currentBullet = Instantiate(bullet).GetComponent<Bullet>();
        currentBullet.Shoot(transform, bulletSpeed, mask);
    }
}