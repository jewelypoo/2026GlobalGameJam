using UnityEngine;
using System.Collections.Generic;

public class PipePuzzle : BasePuzzleObject
{
    [Header("Pipe Puzzle")]

    [SerializeField] public int
        width = 5,
        height = 5;
    [SerializeField] private PipePuzzleGenerator generator;
    public GameObject 
        pipeTJunction,
        pipeStraight,
        pipeCurved,
        pipeEndpoint;

    public PipeTile[,] grid;
    public Vector2Int start, end;

    private void Awake()
    {
        if (generator == null)
        {
            Debug.LogError("PipePuzzleGenerator not assigned!");
            return;
        }
        grid = new PipeTile[width, height];
    }

    public override void Start()
    {
        if (pipeTJunction == null ||
            pipeCurved == null ||
            pipeStraight == null)
        {
            Debug.LogError("MISSING PIPE PREFABS");
            return;
        }
        //generator.Generate(this);
        base.Start();
    }

    public void RegisterTile(PipeTile tile, Vector2Int pos)
    {
        grid[pos.x, pos.y] = tile;
    }

    public void OnTileRotate(Vector2Int pos)
    {
        if (IsSolved())
        {
            Debug.Log("Puzzle solved!");
            StopUsing();
        }
    }

    private bool IsSolved()
    {
        HashSet<Vector2Int> visited = new();
        Queue<Vector2Int> open = new();

        open.Enqueue(start);

        while (open.Count > 0)
        {
            var p = open.Dequeue();
            if (visited.Contains(p)) continue;
            visited.Add(p);

            if (p == end)
                return true;

            PipeTile t = grid[p.x, p.y];

            Try(p, t, Dir.North, Vector2Int.up, open);
            Try(p, t, Dir.East, Vector2Int.right, open);
            Try(p, t, Dir.South, Vector2Int.down, open);
            Try(p, t, Dir.West, Vector2Int.left, open);
        }

        return false;
    }

    private void Try(Vector2Int coordinates, PipeTile thisTile, Dir direction, Vector2Int offset, Queue<Vector2Int> queue)
    {
        if (!thisTile.Has(direction)) return;

        Vector2Int targetCoords = coordinates + offset;
        if (!InBounds(targetCoords)) return;

        if (grid[targetCoords.x, targetCoords.y].Has(Opposite(direction)))
            queue.Enqueue(targetCoords);
    }

    private bool InBounds(Vector2Int coordinates)
    {
        return coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < width && coordinates.y < height;
    }

    private Dir Opposite(Dir d)
    {
        switch (d)
        {
            case Dir.North:
                return Dir.South;
            case Dir.South:
                return Dir.North;
            case Dir.East:
                return Dir.West;
            case Dir.West:
                return Dir.East;
            default:
                return Dir.None;
        }
    }
}
