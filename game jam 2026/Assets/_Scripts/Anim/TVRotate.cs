using System.Collections;
using UnityEngine;

public class TVRotate : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 5f;
    public GameObject tutorialText;
    public bool canShowText = true;
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //print(targetRotation.y);
        if (targetRotation.y < .5 && targetRotation.y > -.5)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,turnSpeed * Time.deltaTime);
        }
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //print("tutorial");
        if (canShowText)
        {
            StartCoroutine(Tutorial());
        }
    }
    public IEnumerator Tutorial()
    {
        canShowText = false;
        tutorialText.SetActive(true);
        yield return new WaitForSeconds(10f);
        tutorialText.SetActive(false);
        yield return new WaitForSeconds(3f);
        canShowText= true;
    }
}
