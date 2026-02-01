using UnityEngine;

public class AlienMaskPickup : MonoBehaviour, IInteractable
{
    private bool isUsable = true;
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void OnInteract()
    {
        if (!isUsable) return;
        isUsable = false;
        playerController.canUseMask = true;
        Destroy(gameObject);
    }

    public bool IsUsable() => isUsable;
    public bool IsBeingUsed() => false;
}
