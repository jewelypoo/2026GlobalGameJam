using UnityEngine;

public class BookPaperPickup : MonoBehaviour, IInteractable
{
    private bool isUsable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void OnInteract()
    {
        // maybe this should be placed somewhere...
        if (!isUsable) return;

        playerController.hasBookPage = true;

        // play anim here
        Destroy(gameObject);
    }

    public bool IsUsable() => isUsable;
    public bool IsBeingUsed() => false;
    
}
