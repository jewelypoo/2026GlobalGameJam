using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] private Image
        blackBackground,
        mask;

    private float
        fadeDuration = .3f,
        fadeDelay = .2f;

    private bool
        maskActive = false,
        debounce = false;

    private Vector3
        maskUpPosition = new Vector3(0, 1000, 0),
        maskDownPosition = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    public void ToggleMask()
    {
        if (debounce) return;
        debounce = true;
        maskActive = !maskActive;

        StartCoroutine(AsyncFadeIn(fadeDuration));

        if (maskActive)
        {
            print("mask is now on");
            StartCoroutine(AsyncMaskDown(fadeDuration));
        }
        else
        {
            print("mask is now off");
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
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
