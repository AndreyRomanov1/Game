using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public float BaseAngle = 60f;
    private void FixedUpdate()
    {
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//+ BaseAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
