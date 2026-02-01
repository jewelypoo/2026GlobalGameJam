using TMPro;
using UnityEngine;

public class PadlockSegment : MonoBehaviour
{
    private string[] letters = {"P", "M", "G", "K"};
    [SerializeField] private TMP_Text display;
    [SerializeField] private PadlockPuzzle puzzle;
    [SerializeField] private int segmentIndex = 0;
    public string input;
    private int index = 0;

    private void Start()
    {
        string correctLetter = puzzle.Code[segmentIndex].ToString();
        do
        {
            index = Random.Range(0, letters.Length);
        }
        while (letters[index] == correctLetter);
        UpdateDisplay();
    }

    public void Increase()
    {
        index++;

        if (index >= letters.Length)
            index = 0;

        UpdateDisplay();
        puzzle.CheckSolved();
    }

    public void Decrease()
    {
        index--;

        if (index < 0)
            index = letters.Length - 1;

        UpdateDisplay();
        puzzle.CheckSolved();
    }

    private void UpdateDisplay()
    {
        display.text = letters[index];
    }

    public string GetInput()
    {
        return display.text;
    }
}
