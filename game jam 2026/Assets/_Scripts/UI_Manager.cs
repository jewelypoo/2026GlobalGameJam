using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Image
        blackBackground,
        mask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeIn(float duration = 1f)
    {
        float elapsedTime = 0f;
        Color blackColor = blackBackground.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            blackColor.a = 1f - alpha;
            blackBackground.color = blackColor;
            yield return null;
        }
        // Ensure the final alpha is set to 0
        blackColor.a = 0f;
        blackBackground.color = blackColor;
    }

    private IEnumerator FadeOut(float duration = 1f)
    {
        float elapsedTime = 0f;
        Color blackColor = blackBackground.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            blackColor.a = 1f - alpha;
            blackBackground.color = blackColor;
            yield return null;
        }
        // Ensure the final alpha is set to 0
        blackColor.a = 0f;
        blackBackground.color = blackColor;
    }
}
