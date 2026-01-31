using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    
    private CharacterController controller;
    private UI_Manager uiManager;

    [Header("Movement Settings")]
    [SerializeField]
    private float
        movementSpeed = 10f,
        gravity = 9.81f;

    private Vector2 inputAxis;
    private float verticalVelocity;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private bool
        debounce = false;

    void Start()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("CharacterController component not found on the player object.");
        }
        uiManager = FindFirstObjectByType<UI_Manager>();
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// handles movement for player
    /// </summary>
    private void HandleMovement()
    {
        float forwardInput = inputAxis.y;
        float rightInput = inputAxis.x;

        Vector3 move = new Vector3(rightInput, 0, forwardInput);
        move = mainCamera.transform.TransformDirection(move);

        move *= movementSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }

    /// <summary>
    /// for gravity calculation
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// updates movement input axis
    /// </summary>
    /// <param name="state"></param>
    public void Move(CallbackContext state)
    {
        inputAxis = state.ReadValue<Vector2>();
    }

    public void Mask(CallbackContext state)
    {
        if (state.performed)
        {
            print("you pressed the mask button");
            uiManager.ToggleMask();
        }
    }
}
