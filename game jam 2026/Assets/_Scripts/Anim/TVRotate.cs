using UnityEngine;

public class TVRotate : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 5f;

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        print(targetRotation.y);
        if (targetRotation.y < .5 && targetRotation.y > -.5)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,turnSpeed * Time.deltaTime);
        }
        
    }
}
