using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;
using TMPro;

public class CameraHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float
        interactDistance = 5f;

    [Header("References")]
    [SerializeField] private Image
        cursor;
    [SerializeField] private TMP_Text
        interactText;
    [SerializeField] private Camera mainCamera;



    private string interactKeybindText;
    private TagHandle interactableTagHandle;
    private IInteractable lastHitInteractable;
    private int maskLayer, nonMaskLayer, clickableLayer;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        maskLayer = LayerMask.GetMask("Mask Object");
        nonMaskLayer = LayerMask.GetMask("Nonmask Object");
        clickableLayer = LayerMask.GetMask("Clickable");
        DisableMaskView();
        if (TryGetComponent<PlayerInput>(out PlayerInput inputSystem))
        {
            string keybind = inputSystem.currentActionMap.FindAction("Interact").GetBindingDisplayString(0);
            interactKeybindText = " (" + keybind + ")";
            //print("find input system yay");
        }
        else
        {
            Debug.LogError("CANT FIND INPUT SYSTEM ON CAMERA HANDLER");
            interactKeybindText = " (E)";
        }
        interactableTagHandle = TagHandle.GetExistingTag("Interactable");
    }

    private void LateUpdate()
    {
        CheckForInteractables();
    }

    /// <summary>
    /// shows objects that can only be seen with mask
    /// </summary>
    public void EnableMaskView()
    {
        mainCamera.cullingMask = ~nonMaskLayer;
    }

    /// <summary>
    /// shows objects that can be seen without mask
    /// </summary>
    public void DisableMaskView()
    {
        mainCamera.cullingMask = ~maskLayer;
    }

    private void ShowHitCursor()
    {
        cursor.color = Color.green;
    }
    private void ResetCursorInformation()
    {
        cursor.color = Color.white;
        interactText.text = "";
        lastHitInteractable = null;
    }

    private void CheckForInteractables()
    {
        RaycastHit hit;
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        //Debug.DrawRay(origin, direction * interactDistance, isHit ? Color.green : Color.red, 0.1f);

        if (!Physics.Raycast(origin, direction, out hit, interactDistance, ~clickableLayer) ||
            !hit.collider.gameObject.CompareTag(interactableTagHandle) ||
            !hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable thisInteractable))
        {
            ResetCursorInformation();
            return;
        }

        lastHitInteractable = thisInteractable;

        interactText.text = lastHitInteractable.IsUsable() ? hit.collider.gameObject.name + interactKeybindText : hit.collider.gameObject.name;

        ShowHitCursor();
    }

    public void OnInteractFired(CallbackContext state)
    {
        if (lastHitInteractable == null) return;
        if (playerController.isMovementEnabled == false) return;
        if (state.performed)
        {
            lastHitInteractable.OnInteract();
            
        }
    }

    public void OnClick(CallbackContext state)
    {
        if (state.performed)
        {
            RaycastHit hit;
            //Vector2 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.gameObject.TryGetComponent<IClickable>(out IClickable clickable))
                {
                    clickable.OnClick();
                }
            }
        }
    }
}
