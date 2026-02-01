using UnityEngine;

public class Dialogue : MonoBehaviour, IInteractable
{
    public string dialogue;
    private bool isUsable = true;

    private UI_Manager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<UI_Manager>();
    }

    public void OnInteract()
    {
        manager.StartDialogue(dialogue);
    }

    public bool IsUsable() => isUsable;
    public bool IsBeingUsed() => false;
}
