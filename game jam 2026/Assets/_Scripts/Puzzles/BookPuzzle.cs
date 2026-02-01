using UnityEngine;

public class BookPuzzle : BasePuzzleObject, IClickable
{
    [SerializeField] private GameObject startVersion, endVersion, camera2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        startVersion.SetActive(true);
        endVersion.SetActive(false);
    }

    public override void OnPuzzleComplete()
    {
        puzzleCompleted = true;
        camera2.SetActive(true);
        startVersion.SetActive(true);
        endVersion.SetActive(true);
    }

    public override void OnInteract()
    {
        if ((!IsUsable() && IsBeingUsed() == false)) return;
        if (MainCamera != null)
        {
            isActive = true;
            MainCamera.SetActive(true);
            playerController.SetCurrentPuzzle(this);
        }
        if (puzzleCompleted)
        {
            camera2.SetActive(true);
        }
    }

    public override void StopUsing()
    {
        base.StopUsing();
        camera2.SetActive(false);
    }

    public void OnClick()
    {
        if (!IsUsable()) return;

        // make sure player is looking at book
        if (playerController.GetCurrentPuzzle == this && playerController.hasBookPage)
        {
            OnPuzzleComplete();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
