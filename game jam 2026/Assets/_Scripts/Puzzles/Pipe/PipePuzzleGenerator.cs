using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PipePuzzleGenerator : MonoBehaviour
{
    private PipePuzzle puzzle;
    private float spacing = .2f;
    /// <summary>
    /// ah yes
    /// </summary>
    /// <param name="puzzle"></param>
    public void Generate(PipePuzzle givenPuzzle)
    {
        puzzle = givenPuzzle;
        int width = puzzle.width;
        int height = puzzle.height;
        
        Vector3 gridOffset = new Vector3((width - 1) * spacing / 2f, (height - 1) * spacing / 2f, 0f);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Vector3 position = transform.position + new Vector3(x * spacing, y * spacing, 0);
                Vector3 localPosition = new Vector3(x * spacing, y * spacing, 0) - gridOffset;
                Vector3 worldPosition = transform.position
                         + transform.right * localPosition.x
                         + transform.up * localPosition.y
                         + transform.forward * localPosition.z;

                GameObject tempTile = Instantiate(puzzle.pipeStraight, worldPosition, Quaternion.identity, puzzle.transform);

                if (!tempTile.TryGetComponent<PipeTile>(out PipeTile tile))
                {
                    Debug.LogError("PipePuzzleGenerator: Could not find PipeTile component on instantiated tile.");
                    return;
                }
                tile.gridPosition = new Vector2Int(x, y);
                tile.puzzle = puzzle;

                puzzle.grid[x, y] = tile;
            }
        }

        puzzle.start = new Vector2Int(0, Random.Range(0, height));
        puzzle.end = new Vector2Int(width - 1, Random.Range(0, height));
        GameObject startTile = Instantiate(puzzle.pipeEndpoint, puzzle.grid[puzzle.start.x, puzzle.start.y].transform.position, Quaternion.identity, puzzle.transform);
        GameObject endTile = Instantiate(puzzle.pipeEndpoint, puzzle.grid[puzzle.end.x, puzzle.end.y].transform.position, Quaternion.identity, puzzle.transform);

        //print("Start: " + puzzle.start + " End: " + puzzle.end);

        // generate random path
        List<Vector2Int> path = new();
        Vector2Int current = puzzle.start;
        path.Add(current);
        
        while (current != puzzle.end)
        {
            List<Vector2Int> options = GetNeighbors(current, width, height);
            current = options[Random.Range(0, options.Count)];
            if (!path.Contains(current))
            {
                path.Add(current);
            }
        }
        for (int i = 0; i < path.Count; i++)
        {
            Dir direction = Dir.None;

            if (i > 0)
            {
                direction |= GetDirectionFrom(path[i], path[i - 1]);
            }

            if (i < path.Count - 1)
            {
                direction |= GetDirectionFrom(path[i], path[i + 1]);
            }
            if (i != 0 && i != path.Count - 1 && GetDirectionCount(direction) < 2)
            {
                direction |= GetDirectionFrom(path[i], path[i + 1]);
            }

            puzzle.grid[path[i].x, path[i].y].connections = direction;
        }

        Dir startDir = Dir.East;
        Dir endDir = Dir.West;

        if (path.Count > 1)
        {
            startDir |= GetDirectionFrom(path[0], path[1]);
            endDir |= GetDirectionFrom(path[^1], path[^2]);
        }

        puzzle.grid[puzzle.start.x, puzzle.start.y].connections = startDir;
        puzzle.grid[puzzle.end.x, puzzle.end.y].connections = endDir;

        // generate random connections for non-path tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PipeTile tile = puzzle.grid[x, y];
                if (tile.connections == Dir.None)
                {
                    tile.connections = GetRandomConnections();
                }
            }
        }

        // replace tiles with correct prefabs and random rotations
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PipeTile oldTile = puzzle.grid[x, y];
                int count = GetDirectionCount(oldTile.connections);
                Dir targetDirection = oldTile.connections;

                GameObject targetPrefab;
                if (count == 3)
                {
                    targetPrefab = puzzle.pipeTJunction;
                }
                else if (IsStraightPipe(oldTile.connections))
                {
                    targetPrefab = puzzle.pipeStraight;
                }
                else
                {
                    targetPrefab = puzzle.pipeCurved;
                }

                PipeTile newTile = Replace(puzzle, oldTile, targetPrefab);

                newTile.ResetToBase();

                OrientTile(newTile, targetDirection);

                int spins = Random.Range(0, 4);
                for (int i = 0; i < spins; i++)
                    newTile.Rotate();

                puzzle.RegisterTile(newTile, newTile.gridPosition);
            }
        }
    }

    /// <summary>
    /// grabs all neighbors at the target coordinates within the grid bounds
    /// </summary>
    /// <param name="coordinates">target coordinate</param>
    /// <param name="gridWitdh">max width of the grid</param>
    /// <param name="gridHeight">max height of the grid</param>
    /// <returns></returns>
    private List<Vector2Int> GetNeighbors(Vector2Int coordinates, int gridWitdh, int gridHeight)
    {
        List<Vector2Int> neighbors = new();

        if (coordinates.x > 0) neighbors.Add(coordinates + Vector2Int.left);
        if (coordinates.x < gridWitdh - 1) neighbors.Add(coordinates + Vector2Int.right);
        if (coordinates.y > 0) neighbors.Add(coordinates + Vector2Int.down);
        if (coordinates.y < gridHeight - 1) neighbors.Add(coordinates + Vector2Int.up);

        return neighbors;
    }

    /// <summary>
    /// makes sure the tile is oriented to match its actual direction(s)
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="targetDirections"></param>
    private void OrientTile(PipeTile tile, Dir targetDirections)
    {
        for (int i = 0; i < 4; i++)
        {
            if (tile.connections == targetDirections)
                return;

            tile.Rotate();
        }

        Debug.LogWarning("Failed to orient tile at " + tile.gridPosition + ", target is "+ targetDirections +" while it is currently " + tile.connections);
        puzzle.grid[tile.gridPosition.x, tile.gridPosition.y] = null;
        Destroy(tile.gameObject);
    }

    private Dir GetDirectionFrom(Vector2Int a, Vector2Int b)
    {
        Vector2Int d = b - a;

        if (d == Vector2Int.up) return Dir.North;
        if (d == Vector2Int.right) return Dir.East;
        if (d == Vector2Int.down) return Dir.South;
        if (d == Vector2Int.left) return Dir.West;

        return Dir.None;
    }

    private int GetDirectionCount(Dir d)
    {
        int count = 0;
        if ((d & Dir.North) != 0) count++;
        if ((d & Dir.East) != 0) count++;
        if ((d & Dir.South) != 0) count++;
        if ((d & Dir.West) != 0) count++;
        return count;
    }

    private bool IsStraightPipe(Dir d)
    {
        bool vertical =
            (d & Dir.North) != 0 &&
            (d & Dir.South) != 0;

        bool horizontal =
            (d & Dir.East) != 0 &&
            (d & Dir.West) != 0;

        return vertical || horizontal;
    }
    private Dir GetRandomConnections()
    {
        switch (Random.Range(0, 2))
        {
            case 0: return Dir.East | Dir.West;
            default: return Dir.South | Dir.West;
        }
    }

    private PipeTile Replace(PipePuzzle puzzle, PipeTile oldTile, GameObject prefab)
    {
        Vector3 position = oldTile.transform.position;
        Vector2Int gridPosition = oldTile.gridPosition;
        Transform parent = oldTile.transform.parent;

        Destroy(oldTile.gameObject);

        PipeTile tile = Instantiate(prefab, position, Quaternion.identity, parent).GetComponent<PipeTile>();

        tile.gridPosition = gridPosition;
        tile.puzzle = puzzle;

        puzzle.grid[tile.gridPosition.x, tile.gridPosition.y] = tile;

        return tile;
    }
}
