using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EndTextMove : MonoBehaviour
{
    public float moveSpeed;
    private float distance;

    [SerializeField] private GameObject quitCanvas;

    private void Start()
    {
        quitCanvas.SetActive(false);
        StartCoroutine(DelayShow());
    }

    // Update is called once per frame
    void Update()
    {
        distance += moveSpeed * Time.deltaTime * .1f;
        transform.position = transform.position + new Vector3(0, distance, 0);
    }

    private IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(20f);
        quitCanvas.SetActive(true);
    }
}
