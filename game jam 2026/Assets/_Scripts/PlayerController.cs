using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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

    

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private bool
        debounce = false;
    private Vector2 inputAxis;
    private float verticalVelocity;
    private string interactKeybindText;
    private TagHandle interactableTagHandle;

    void Start()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("CharacterController component not found on the player object.");
        }
        uiManager = FindFirstObjectByType<UI_Manager>();
        if (TryGetComponent<PlayerInput>(out PlayerInput inputSystem))
        {
            string keybind = inputSystem.currentActionMap.FindAction("Interact").GetBindingDisplayString(0);
            interactKeybindText = " (" + keybind + ")";
        }
        else
        {
            interactKeybindText = " (E)";
        }
        interactableTagHandle = TagHandle.GetExistingTag("Interactable");
    }

    private void Update()
    {
        HandleMovement();
        //CheckForInteractables();
    }

    /*
    private void CheckForInteractables()
    {
        RaycastHit hit;
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        //Debug.DrawRay(origin, direction * interactDistance, isHit ? Color.green : Color.red, 0.1f);

        if (!Physics.Raycast(origin, direction, out hit, interactDistance, InteractableLayerMask) ||
            !hit.collider.gameObject.CompareTag(interactableTagHandle) ||
            !hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable thisInteractable))
        {
            ResetCursorInformation();
            return;
        }

        lastHitInteractable = thisInteractable;

        interactText.text = lastHitInteractable.IsUsable() ? hit.collider.gameObject.name + keybindText : hit.collider.gameObject.name;

        ShowHitCursor();
    }
    */
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
