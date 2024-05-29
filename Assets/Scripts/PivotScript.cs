using System.Collections;
using UnityEngine;

public class PivotScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FixedUpdateCoroutine());
    }

    private IEnumerator FixedUpdateCoroutine()
    {
        yield return null;
        while (true)
        {
            var direction = (CurrentGame.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position)
                .normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //+ BaseAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);

            yield return new WaitForFixedUpdate();
        }
    }
}