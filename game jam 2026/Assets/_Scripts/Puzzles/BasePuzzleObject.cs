using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BasePuzzleObject : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private bool
        canBeUsed = false;

    [Header("References")]
    [SerializeField] private GameObject 
        cameraGameObject;

    protected bool isActive = false;
    [HideInInspector] protected bool puzzleCompleted = false;

    protected PlayerController playerController;
    protected GameObject MainCamera
    {
        get { return cameraGameObject; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("playercontroller not found in scene.");
        }
        if (cameraGameObject != null)
        {
            cameraGameObject.SetActive(false);
        }
    }

    public virtual void OnInteract()
    {
        if ((!IsUsable() && IsBeingUsed() == false) || puzzleCompleted == true) return;
        //print("now using " + transform.name);
        if (cameraGameObject != null)
        {
            isActive = true;
            cameraGameObject.SetActive(true);
            playerController.SetCurrentPuzzle(this);
        }
    }

    public virtual void StopUsing()
    {
        if (cameraGameObject != null)
        {
            isActive = false;
            cameraGameObject.SetActive(false);
            playerController.StopCurrentPuzzle();
        }
    }

    public virtual void OnPuzzleComplete()
    {
        puzzleCompleted = true;
        canBeUsed = false;
        StopUsing();
    }

    public bool IsUsable() => canBeUsed;
    public bool IsBeingUsed() => isActive;
}
