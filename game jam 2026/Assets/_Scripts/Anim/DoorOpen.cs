using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(waitForOpen());
        }
    }

   public IEnumerator waitForOpen()
    {
        GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
