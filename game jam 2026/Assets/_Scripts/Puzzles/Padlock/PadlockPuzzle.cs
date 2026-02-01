using TMPro;
using UnityEngine;

public class PadlockPuzzle : BasePuzzleObject
{
    [SerializeField] private GameObject padlockCanvasObject;

    [SerializeField] private PadlockSegment[] segments;

    [HideInInspector] private string code = "PMGK";

    public string Code => code;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        padlockCanvasObject.SetActive(false);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        padlockCanvasObject.SetActive(true);
    }

    public override void StopUsing()
    {
        base.StopUsing();
        padlockCanvasObject.SetActive(false );
    }

    

    public void CheckSolved()
    {
        string inputString = string.Empty;

        for (int i = 0; i < segments.Length; i++)
        {
            inputString += segments[i].GetInput();
        }
        //print(inputString);
        //print(code);
        if (inputString == code)
        {
            OnPuzzleComplete();
        }
    }
}
