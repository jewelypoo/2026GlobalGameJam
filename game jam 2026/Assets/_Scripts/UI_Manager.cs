using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] private Image
        blackBackground,
        mask,
        cursorImage;

    [SerializeField] private GameObject
        interactTextGameObject, exitButtonGameObject,
        dialogueTextbox, dialogueContinueBox;

    [SerializeField] private TMP_Text dialogueTextDisplay;
    [SerializeField] private AudioSource dialogueAudio;

    private float
        fadeDuration = .3f,
        fadeDelay = .2f;

    private bool
        debounce = false;

    private Vector3
        maskUpPosition = new Vector3(0, 2500, 0),
        maskDownPosition = new Vector3(0, 0, 0);

    private CameraHandler cameraHandler;
    private PlayerController playerController;

    private Coroutine dialogueThread;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        cameraHandler = FindFirstObjectByType<CameraHandler>();
        HideMouse();
        if (blackBackground == null)
        {
            Debug.LogError("Black Background Image is not assigned in the inspector.");
            return;
        }
        if (mask == null)
        {
            Debug.LogError("Mask Image is not assigned in the inspector.");
            return;
        }
        blackBackground.gameObject.SetActive(true);
        SetAlpha(blackBackground, 0f);
        mask.rectTransform.anchoredPosition = maskUpPosition;
        ResetDialogue();
    }

    public void ToggleMask()
    {
        if (debounce) return;
        debounce = true;
        playerController.maskActive = !playerController.maskActive;

        StartCoroutine(AsyncFadeIn(fadeDuration));

        if (playerController.maskActive)
        {
            //print("mask is now on");
            StartCoroutine(AsyncMaskDown(fadeDuration));
        }
        else
        {
            //print("mask is now off");
            StartCoroutine(AsyncMaskUp(fadeDuration));
        }
    }

    private IEnumerator AsyncFadeIn(float duration = 1f)
    {
        float elapsedTime = 0f;
        Color blackColor = blackBackground.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            SetAlpha(blackBackground, alpha);
            yield return null;
        }
        SetAlpha(blackBackground, 1f);

        yield return new WaitForSeconds(fadeDelay);

        StartCoroutine(AsyncFadeOut(duration));
    }

    private IEnumerator AsyncFadeOut(float duration = 1f)
    {
        float elapsedTime = 0f;
        Color blackColor = blackBackground.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            SetAlpha(blackBackground, 1f - alpha);
            yield return null;
        }
        SetAlpha(blackBackground, 0f);
        debounce = false;
    }

    private IEnumerator AsyncMaskDown(float duration = 1f)
    {
        mask.rectTransform.anchoredPosition = maskUpPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            mask.rectTransform.anchoredPosition = Vector3.Lerp(maskUpPosition, maskDownPosition, alpha);

            yield return null;
        }
        mask.rectTransform.anchoredPosition = maskUpPosition;
        cameraHandler.EnableMaskView();
    }

    private IEnumerator AsyncMaskUp(float duration = 1f)
    {
        mask.rectTransform.anchoredPosition = maskDownPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            mask.rectTransform.anchoredPosition = Vector3.Lerp(maskDownPosition, maskUpPosition, alpha);
            yield return null;
        }
        cameraHandler.DisableMaskView();
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    public void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cursorImage.enabled = false;
        interactTextGameObject.SetActive(false);
        exitButtonGameObject.SetActive(true);
    }

    public void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorImage.enabled = true;
        interactTextGameObject.SetActive(true);
        exitButtonGameObject.SetActive(false);
    }

    private void ResetDialogue()
    {
        dialogueContinueBox.SetActive(false);
        dialogueTextDisplay.text = string.Empty;
        if (dialogueAudio != null)
        {
            dialogueAudio.Stop();
        }
    }

    public void StartDialogue(string givenDialogue)
    {
        // check to see if there is ongoing dialogue and override it

        if (dialogueThread != null)
        {
            StopCoroutine(dialogueThread);
            dialogueThread = null;
            dialogueTextDisplay.text = string.Empty;
        }

        dialogueThread = StartCoroutine(TypewriterEffect(givenDialogue));
    }

    private IEnumerator TypewriterEffect(string givenText)
    {
        if (dialogueAudio != null)
        {
            dialogueAudio.Play();
        }
        dialogueContinueBox.SetActive(false);

        dialogueTextDisplay.text = givenText;
        dialogueTextDisplay.maxVisibleCharacters = 0;

        for (int i = 0; i < givenText.Length; i++)
        {
            dialogueTextDisplay.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(0.02f);
        }

        dialogueContinueBox.SetActive(true);
        if (dialogueAudio != null)
        {
            dialogueAudio.Stop();
        }
        dialogueThread = null;
    }

    public void OnDialogueContinue()
    {
        if (dialogueThread != null)
        {
            //print("stopping current thread");
            StopCoroutine(dialogueThread); 
            dialogueThread = null;
            dialogueTextDisplay.maxVisibleCharacters = dialogueTextDisplay.text.Length;
            dialogueContinueBox.SetActive(true);
        }
        else
        {
            //print("stopping dialogue");
            ResetDialogue();
        }
    }
}
