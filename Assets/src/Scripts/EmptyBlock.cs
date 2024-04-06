using UnityEngine;

public class EmptyBlock : MonoBehaviour
{
    public GridScript generator;
    public int x;
    public int y;

    private void Start()
    {
        generator = GetComponentInParent<GridScript>();
        var position = transform.position;
        x = (int)position.x / GridScript.BlockWidth;
        y = (int)position.y / GridScript.BlockHeight;
    }

    private void Update()
    {
    }
}