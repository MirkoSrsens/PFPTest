using UnityEngine;

public class Node
{
    public Vector2Int position { get; set; }

    public Vector2Int index { get; set; }

    public Node parent { get; set; }

    public float FCost { get; set; }

    public float HCost { get; set; }

    public float GCost { get { return FCost + HCost; } }

    public Node(Vector2Int position, Vector2Int index)
    {
        this.position = position;
        this.index = index;
    }
}
