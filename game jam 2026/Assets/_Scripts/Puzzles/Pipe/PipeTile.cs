using UnityEngine;


public class PipeTile : MonoBehaviour, IClickable
{
    public PipePuzzle parentPuzzle;
    public bool isLocked = false;
    public bool isStraight = false;

    private int currentRotationSteps = 0;
    private int correctRotationSteps = 0;

    private void Awake()
    {
        int spins = Random.Range(0, 4);

        if (isLocked)
        {
            return;
        }
        for (int i = 0; i < spins; i++)
            Rotate();
    }

    public void OnClick()
    {
        if (isLocked)
        {
            return;
        }
        Rotate();
        if (parentPuzzle != null)
        {
            parentPuzzle.OnTileRotate(this);
        }
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);
        currentRotationSteps = (currentRotationSteps + 1) % 4;
    }

    public float GetRotation()
    {
        return transform.eulerAngles.x;
    }

    public bool IsCorrectlyOriented()
    {
        if (isStraight)
        {
            return currentRotationSteps % 2 == correctRotationSteps % 2;
        }
        return currentRotationSteps == correctRotationSteps;
    }
}
