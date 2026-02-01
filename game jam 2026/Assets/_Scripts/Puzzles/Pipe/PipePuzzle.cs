using UnityEngine;
using System.Collections.Generic;

public class PipePuzzle : BasePuzzleObject
{
    [Header("Pipe Puzzle")]

    public PipeTile[] pipes;

    private int totalPipeCount;
    public GameObject frontPanel;
    public GameObject card;
    [SerializeField] private AudioSource winSound;

    public override void Start()
    {
        base.Start();
        totalPipeCount = pipes.Length;
        foreach (PipeTile pipe in pipes)
        {
            pipe.parentPuzzle = this;
        }
    }

    public void OnTileRotate(PipeTile tile)
    {
        // if tile == correct rotation
        if (IsPuzzleComplete())
        {
            foreach (PipeTile pipe in pipes)
            {
                pipe.isLocked = true;
            }
            OnPuzzleComplete();
            //print("Pipe Puzzle Complete!");
        }
    }


    private bool IsPuzzleComplete()
    {
        foreach (PipeTile pipe in pipes)
        {
            if (!pipe.IsCorrectlyOriented())
            {
                //print(pipe.name + " is not correctly oriented.");
                return false;
            }
        }
        return true;
    }
    public override void OnInteract()
    {
        base.OnInteract();
        GetComponent<BoxCollider>().enabled = false;
    }

    public override void StopUsing()
    {
        base.StopUsing();
        GetComponent<BoxCollider>().enabled = true;
    }

    public override void OnPuzzleComplete()
    {
        base.OnPuzzleComplete();
        frontPanel.SetActive(false);
        card.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = false;
        if (winSound != null)
        {
            winSound.Play();
        }
    }
}
