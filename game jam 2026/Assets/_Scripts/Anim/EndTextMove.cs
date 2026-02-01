using UnityEngine;

public class EndTextMove : MonoBehaviour
{
    public float moveSpeed;
    private float distance;

    // Update is called once per frame
    void Update()
    {
        distance += moveSpeed * Time.deltaTime * .1f;
        transform.position = transform.position + new Vector3(0, distance, 0);
    }
}
