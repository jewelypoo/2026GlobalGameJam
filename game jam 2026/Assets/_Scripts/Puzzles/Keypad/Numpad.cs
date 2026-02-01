using UnityEngine;

public class Numpad : MonoBehaviour, IClickable
{
    [SerializeField] private int num = -1;
    [SerializeField] private bool
        isEnter = false,
        isClear;
    [SerializeField] KeypadPuzzle keyPuzzle;
    public void OnClick()
    {
        if (num >= 0 && num <= 9)
        {
            keyPuzzle.NumberPressed(num);
        }

        if (isEnter)
        {
            keyPuzzle.Enter();
        }
        if (isClear)
        {
            keyPuzzle.OnClearPressed();
        }
    }
}
