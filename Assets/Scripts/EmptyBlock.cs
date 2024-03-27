using UnityEngine;

public class EmptyBlock : MonoBehaviour
{
    public Grid generator;
    public int x;
    public int y;

    private void Start()
    {
        generator = GetComponentInParent<Grid>();
        var position = transform.position;
        x = (int)position.x / generator.blockWidth;
        y = (int)position.y / generator.blockHeight;
    }

    private void Update()
    {
    }
}