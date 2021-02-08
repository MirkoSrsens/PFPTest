using UnityEngine;

public class Node
{
    public Vector2 position { get; set; }

    public Vector2Int index { get; set; }

    public Node parent { get; set; }

    public float FCost { get; set; }

    public float HCost { get; set; }

    public float GCost { get { return FCost + HCost; } }

    public bool IsWalkable { get; set; }

    public Node(Vector2 position, Vector2Int index)
    {
        this.IsWalkable = true;
        this.position = position;
        this.index = index;
    }
}
