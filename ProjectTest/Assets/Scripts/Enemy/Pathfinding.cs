using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    private Node[,] cells;

    private Vector3Int startPoint { get; set; }

    public Transform start;

    public Transform end;

    private void Awake()
    {
        startPoint = tileMap.cellBounds.min;
        var end = tileMap.cellBounds.max;
        var size = startPoint - end;
        cells = new Node[Mathf.Abs(size.x), Mathf.Abs(size.y)];

        for(int i =0; i<cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                var node = new Node(new Vector2(tileMap.cellBounds.min.x + i - tileMap.tileAnchor.x, tileMap.cellBounds.min.y + j - tileMap.tileAnchor.y), new Vector2Int(i, j));

                var hit = Physics2D.OverlapCircleAll(node.position, 0.1f);

                if(hit.Length > 0)
                {
                    node.IsWalkable = false;
                }

                cells[i,j] = node;
            }
        }

        GetPath(start, this.end);
    }

    public void GetPath(Transform current, Transform target)
    {
        var startPoint = GetClosestNode(current.position);
        var targetPoint = GetClosestNode(target.position);

        var openHash = new Queue<Node>();
        var closedHash = new HashSet<Node>();
        var selectedNode = startPoint;

        openHash.Enqueue(startPoint);

        var count = 0;
        while (openHash.Count > 0 && count < cells.Length)
        {
            var nextNode = openHash.Dequeue();

            nextNode.FCost = Vector2.Distance(startPoint.position, nextNode.position);
            nextNode.HCost = Vector2.Distance(targetPoint.position, nextNode.position);

            CalculateNeighbours(openHash, closedHash, nextNode);

            selectedNode = nextNode;
            closedHash.Add(nextNode);
            count++;

            if(selectedNode == targetPoint)
            {
                break;
            }
        }

        count = 0;
        while (count < 1000 && (selectedNode != startPoint && selectedNode != null))
        {
            Instantiate(end, new Vector3(selectedNode.position.x, selectedNode.position.y, 0), Quaternion.identity, null);
            selectedNode = selectedNode.parent;
        }
    }

    private void CalculateNeighbours(Queue<Node> openHash, HashSet<Node> closedHash, Node nextNode)
    {
        var hasLeft = nextNode.index.x - 1 > 0;
        var hasRight = nextNode.index.x + 1 < cells.GetLength(0);

        var hasDown = nextNode.index.y - 1 > 0;
        var hasUp = nextNode.index.y + 1 < cells.GetLength(1);

        if (hasLeft)
        {
            var node = cells[nextNode.index.x - 1, nextNode.index.y];

            if (!closedHash.Contains(node) && !openHash.Contains(node) && node.IsWalkable)
            {
                openHash.Enqueue(cells[nextNode.index.x - 1, nextNode.index.y]);

                if (node.parent == null || node.parent.GCost > nextNode.GCost)
                {
                    node.parent = nextNode;
                }
            }
        }
        if (hasRight)
        {
            var node = cells[nextNode.index.x + 1, nextNode.index.y];

            if (!closedHash.Contains(node) && !openHash.Contains(node) && node.IsWalkable)
            {
                openHash.Enqueue(node);

                if (node.parent == null || node.parent.GCost > nextNode.GCost)
                {
                    node.parent = nextNode;
                }
            }
        }
        if (hasUp)
        {
            var node = cells[nextNode.index.x, nextNode.index.y + 1];

            if (!closedHash.Contains(node) && !openHash.Contains(node) && node.IsWalkable)
            {
                openHash.Enqueue(node);

                if (node.parent == null || node.parent.GCost > nextNode.GCost)
                {
                    node.parent = nextNode;
                }
            } 
        }
        if (hasDown)
        {
            var node = cells[nextNode.index.x, nextNode.index.y - 1];

            if (!closedHash.Contains(node) && !openHash.Contains(node) && node.IsWalkable)
            {
                openHash.Enqueue(node);

                if (node.parent == null || node.parent.GCost > nextNode.GCost)
                {
                    node.parent = nextNode;
                }
            }
        }
    }

    private Node GetClosestNode(Vector2 position)
    {
        var x = (int)position.x - startPoint.x;
        var y = (int)position.y - startPoint.y;

        return cells[x, y];
    }
}
