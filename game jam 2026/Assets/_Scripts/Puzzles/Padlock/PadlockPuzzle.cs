using TMPro;
using UnityEngine;

public class PadlockPuzzle : BasePuzzleObject
{
    [SerializeField] private GameObject padlockCanvasObject, openBox, closedBox;

    [SerializeField] private PadlockSegment[] segments;

    [HideInInspector] private string code = "PMGK";

    public string Code => code;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        openBox.SetActive(false);
        closedBox.SetActive(true);
        padlockCanvasObject.SetActive(false);
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (!IsUsable()) return;
        padlockCanvasObject.SetActive(true);
    }

    public override void StopUsing()
    {
        base.StopUsing();
        padlockCanvasObject.SetActive(false );
    }

    public override void OnPuzzleComplete()
    {
        base.OnPuzzleComplete();
        openBox.SetActive(true);
        closedBox.SetActive(false);
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
