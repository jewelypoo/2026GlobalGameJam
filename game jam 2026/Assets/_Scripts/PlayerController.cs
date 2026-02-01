using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    
    private CharacterController controller;
    private UI_Manager uiManager;
    private CameraHandler cameraHandler;

    [Header("Movement Settings")]
    [SerializeField]
    private float
        movementSpeed = 10f,
        gravity = 9.81f;

    

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private Vector2 inputAxis;
    private float verticalVelocity;
    private BasePuzzleObject currentPuzzleObject;

    public bool
        canUseMask = false,
        maskActive = false;

    public bool isMovementEnabled
    {
        get { return controller.enabled; }
    }
    public bool isInteractingWithPuzzle
    {
        get { return currentPuzzleObject != null; }
    }

    void Start()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("CharacterController component not found on the player object.");
        }
        uiManager = FindFirstObjectByType<UI_Manager>();
        cameraHandler = FindAnyObjectByType<CameraHandler>();

        ResumePlayerMovement();
    }

    public void StopPlayerMovement()
    {
        controller.enabled = false;
        uiManager.ShowMouse();
    }

    public void ResumePlayerMovement()
    {
        controller.enabled = true;
        uiManager.HideMouse();
    }

    public void SetCurrentPuzzle(BasePuzzleObject puzzleObject)
    {
        currentPuzzleObject = puzzleObject;
        StopPlayerMovement();
    }
    public void StopCurrentPuzzle()
    {
        if (currentPuzzleObject != null)
        {
            currentPuzzleObject = null;
        }
        ResumePlayerMovement();
    }

    /// <summary>
    /// handles movement for player
    /// </summary>
    private void HandleMovement()
    {
        if (isMovementEnabled == false) return;
        float forwardInput = inputAxis.y;
        float rightInput = inputAxis.x;

        Vector3 move = new Vector3(rightInput, 0, forwardInput);
        move = mainCamera.transform.TransformDirection(move);

        move *= movementSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }

    private void Update()
    {
        HandleMovement();
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
        if (!canUseMask) return;
        if (state.performed)
        {
            print("you pressed the mask button");
            uiManager.ToggleMask();
        }
    }

    public void ExitPuzzle()
    {
        if (currentPuzzleObject == null) return;
        currentPuzzleObject.StopUsing();
    }

    public void QuitPuzzle(CallbackContext state)
    {
        if (state.performed)
        {
            ExitPuzzle();
        }
    }

    public void NumberPressed(CallbackContext state)
    {
        if (currentPuzzleObject == null) return;
        if (state.performed)
        {
            if (int.TryParse(state.control.name, out int number))
            {
                if (currentPuzzleObject.TryGetComponent<KeypadPuzzle>(out KeypadPuzzle keypadPuzzle))
                {
                    keypadPuzzle.NumberPressed(number);
                }
            }
        }
    }

    public void Backspace(CallbackContext state)
    {
        if (currentPuzzleObject == null) return;
        if (state.performed)
        {
            if (currentPuzzleObject.TryGetComponent<KeypadPuzzle>(out KeypadPuzzle keypadPuzzle))
            {
                keypadPuzzle.OnClearPressed();
            }
        }
    }

    public void Enter(CallbackContext state)
    {
        if (currentPuzzleObject == null) return;
        if (state.performed)
        {
            if (currentPuzzleObject.TryGetComponent<KeypadPuzzle>(out KeypadPuzzle keypadPuzzle))
            {
                keypadPuzzle.Enter();
            }
        }
    }
}
