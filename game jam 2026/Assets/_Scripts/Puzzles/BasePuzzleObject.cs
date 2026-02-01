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

    private bool isActive = false;

    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("playercontroller not found in scene.");
        }
        cameraGameObject.SetActive(false);
    }

    public virtual void OnInteract()
    {
        if (!IsUsable() && IsBeingUsed() == false) return;
        print("now using " + transform.name);
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

    public bool IsUsable() => canBeUsed;
    public bool IsBeingUsed() => isActive;
}
