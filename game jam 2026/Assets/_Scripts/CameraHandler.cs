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
    private int
        InteractableLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        InteractableLayerMask = LayerMask.GetMask("Interactable");
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

    private void LateUpdate()
    {
        CheckForInteractables();
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

        if (!Physics.Raycast(origin, direction, out hit, interactDistance) ||
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
        print("yo");
        if (lastHitInteractable == null) return;
        print("trying interact");
        if (state.performed)
        {
            lastHitInteractable.OnInteract();
            print("fired");
        }
    }
}
