using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    private void OnEnable()
    {
        EventBus<DoorEvent>.Subscribe(OnDoorUsed);
    }

    private void OnDisable()
    {
        EventBus<DoorEvent>.Unsubscribe(OnDoorUsed);
    }

    private void OnDoorUsed(DoorEvent doorEvent)
    {
        StartCoroutine(FadeEffect());
    }

    private IEnumerator FadeEffect()
    {
        float elapsedTime = 0f;
        // **FADE OUT**
        while (elapsedTime < fadeDuration / 2)
        {
            fadeImage.color = new Color(0, 0, 0, elapsedTime / (fadeDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // Tamamen karart

        yield return new WaitForSeconds(0.5f); // Kapıdan geçiş süresi

        elapsedTime = 0f;

        // **FADE IN**
        while (elapsedTime < fadeDuration / 2)
        {
            fadeImage.color = new Color(0, 0, 0, 1 - (elapsedTime / (fadeDuration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // Tamamen aç
    
    }
}
