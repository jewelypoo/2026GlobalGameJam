using System.Collections;
using UnityEngine;

public class BookPaperPickup : MonoBehaviour, IInteractable
{
    private bool isUsable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private PlayerController playerController;
    private UI_Manager manager;
    [SerializeField] private AudioSource paperSound;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        manager = FindFirstObjectByType<UI_Manager>();
    }

    public void OnInteract()
    {
        // maybe this should be placed somewhere...
        if (!isUsable) return;

        playerController.hasBookPage = true;
        isUsable = false;
        manager.StartDialogue("Neep Bzorp. Zoogle record #*&@$*@& retrieved. One of my favorite stimulating activities. You should store it somewhere.");

        if (paperSound != null)
        {
            paperSound.Play();
        }
        
        GetComponent<Collider>().enabled = false;
        transform.SetParent(playerController.GetMainCamera.transform);
        transform.localPosition = new Vector3(0.291999996f, -0.170000002f, 0.368999988f);
        transform.localEulerAngles = new Vector3(74.766571f, 237.789551f, 39.7344055f);
        transform.localScale = new Vector3(0.305359989f, 0.213049978f, 0.213049993f);
        //StartCoroutine(PaperAnimation());
    }

    public bool IsUsable() => isUsable;
    public bool IsBeingUsed() => false;
    

    /*
    private IEnumerator PaperAnimation()
    {
        transform.SetParent(playerController.GetMainCamera.transform);
        transform.localPosition = new Vector3(0.291999996f, -0.170000002f, 0.368999988f);
        transform.localEulerAngles = new Vector3(74.766571f, 237.789551f, 39.7344055f);
        transform.localScale = new Vector3(0.305359989f, 0.213049978f, 0.213049993f);
        yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
    }
    */
}
