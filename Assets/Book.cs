using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    // Reference to the UI CanvasGroup (for smooth fade)
    public CanvasGroup uiCanvasGroup;
    
    // Toggle speed for the fade effect
    public float fadeDuration = 0.5f;

    // Is the UI currently visible?
    private bool isVisible = false;

    private Coroutine fadeCoroutine;

    void Update()
    {
        // Check if the 'R' key is pressed to toggle UI visibility
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleUIVisibility();
        }
    }

    // Function to toggle the UI visibility
    public void ToggleUIVisibility()
    {
        // Toggle visibility flag
        isVisible = !isVisible;

        // If there is a fading process already running, stop it
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start the fading process
        fadeCoroutine = StartCoroutine(FadeUI(isVisible));
    }

    // Function to close the UI (set invisible)
    public void CloseUI()
    {
        if (isVisible)
        {
            // Ensure the UI is currently visible, then close it
            isVisible = false;

            // If there is a fading process already running, stop it
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            // Start the fading process to hide the UI
            fadeCoroutine = StartCoroutine(FadeUI(isVisible));
        }
    }

    // Coroutine to fade the UI in or out
    private IEnumerator FadeUI(bool show)
    {
        float startAlpha = uiCanvasGroup.alpha;
        float endAlpha = show ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            uiCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        uiCanvasGroup.alpha = endAlpha;
        uiCanvasGroup.interactable = show;
        uiCanvasGroup.blocksRaycasts = show;
    }
}
