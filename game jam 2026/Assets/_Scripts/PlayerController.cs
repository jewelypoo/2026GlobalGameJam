using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField]
    private float
        movementSpeed = 10f,
        gravity = 9.81f;

    private Vector2 inputAxis;
    private float verticalVelocity;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("CharacterController component not found on the player object.");
        }
    }

    private void Update()
    {
        float forwardInput = inputAxis.y;
        float rightInput = inputAxis.x;

        Vector3 move = new Vector3(rightInput, 0, forwardInput);
        move = mainCamera.transform.TransformDirection(move);

        move *= movementSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    public void Move(CallbackContext state)
    {
        inputAxis = state.ReadValue<Vector2>();
    }
}
