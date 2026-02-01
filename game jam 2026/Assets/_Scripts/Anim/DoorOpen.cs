using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour, IInteractable
{
    private bool usable = true;
    [SerializeField] private AudioSource doorSound;

    private IEnumerator WaitForOpen()
    {
        GetComponent<Animator>().enabled = true;
        doorSound.Play();
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<BoxCollider>().enabled = false;

    }


    public void OnInteract()
    {
        if (!usable) return;
        usable = false;
        StartCoroutine(WaitForOpen());
    }

    public bool IsUsable() => usable;
    public bool IsBeingUsed()
    {
        return false;
    }
}
