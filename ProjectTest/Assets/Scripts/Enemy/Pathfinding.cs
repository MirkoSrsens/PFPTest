using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    private Vector2Int[,] cells;

    private void Awake()
    {
        var start = tileMap.cellBounds.min;
        var end = tileMap.cellBounds.max;
        var size = start - end;
        cells = new Vector2Int[Mathf.Abs(size.x), Mathf.Abs(size.y)];

        for(int i =0; i<cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i,j] = new Vector2Int(i, j);
            }
        }
    }

    public static void GetPath(this Transform current, Transform target)
    {

    }
}
