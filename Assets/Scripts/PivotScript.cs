using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PivotScript : MonoBehaviour
{
    // public float BaseAngle = 60f;
    // private Camera mainCamera;

    private void Start()
    {
        StartCoroutine(FixedUpdateCoroutine());
    }
    

    private IEnumerator FixedUpdateCoroutine()
    {
        yield return null;
        var mainCamera = Camera.main;

        while (true)
        {
            var direction = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//+ BaseAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);

            yield return new WaitForFixedUpdate();
        }
    }
    

    // private void FixedUpdate()
    // {
    //     var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    //     var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//+ BaseAngle;
    //     transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    // }
}
