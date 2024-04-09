using Unity.VisualScripting;
using UnityEngine;

public static class Tools
{
    public static bool FindObjectOnLine(Vector2 startPosition, Vector2 endPosition, int mask, out GameObject result)
    {
        result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
        return result is not null;
    }
}