using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class KeypadPuzzle : BasePuzzleObject
{
    [Header("Keypad Puzzle Settings")]
    [SerializeField] private int 
        codeLength = 4;
    [SerializeField] TMP_Text displayText;
    
    private string code = "1414", input;

    private bool debounce = false;
    private int keysPressed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        // generate a random code
        Clear();
    }

    public void NumberPressed(int number)
    {
        if (debounce) return;
        keysPressed++;
        if (keysPressed > codeLength)
        {
            return;
        }
        string num = number.ToString();
        // handle number press logic
        print("number pressed: " + number);

        input += num;
        displayText.text = input;
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

    private void Clear()
    {
        keysPressed = 0;
        input = string.Empty;
        displayText.text = input;
    }

    public void OnClearPressed()
    {
        if (debounce) return;
        Clear();
    }

    public void Enter()
    {
        if (debounce) return;
        debounce = true;
        if (input == code)
        {
            displayText.text = "CLEAR!";
            
            OnPuzzleComplete();
            StartCoroutine(OnComplete());
        }
        else
        {
            StartCoroutine(DebounceDelay());
        }
    }

    private IEnumerator DebounceDelay()
    {
        displayText.text = "WRONG";
        yield return new WaitForSeconds(.67f);

        Clear();

        debounce = false;
    }

    private IEnumerator OnComplete()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
