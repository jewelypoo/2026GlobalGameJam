using UnityEngine;

/// <summary>
/// directions that the piece is currently connected to
/// bitwise flags, neat stuff
/// </summary>
[System.Flags]
public enum  Dir
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public class PipeTile : MonoBehaviour, IClickable
{
    public Vector2Int gridPosition;
    [HideInInspector] public PipePuzzle puzzle;

    public bool isLocked = false;
    public Dir baseConnections;
    [HideInInspector] public Dir connections;

    public void OnClick()
    {
        /*
        if (!puzzle.IsBeingUsed())
        {
            return;
        }*/
        if (isLocked)
        {
            return;
        }
        //print("clicked pipe tile at " + gridPosition);
        Rotate();
        puzzle.OnTileRotate(gridPosition);
    }

    public void Rotate()
    {
        if (gameObject == null)
        {
            return;
        }
        transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);
        connections = RotateData(connections);
    }

    private Dir RotateData(Dir d)
    {
        Dir result = Dir.None;

        if ((d & Dir.North) != 0) result |= Dir.East;
        if ((d & Dir.East) != 0) result |= Dir.South;
        if ((d & Dir.South) != 0) result |= Dir.West;
        if ((d & Dir.West) != 0) result |= Dir.North;

        return result;
    }

    public bool Has(Dir d) => (connections & d) != 0;

    public void ResetToBase()
    {
        transform.localRotation = Quaternion.identity;
        connections = baseConnections;
    }
}
