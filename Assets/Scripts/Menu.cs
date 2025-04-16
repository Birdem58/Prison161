using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    private protected CanvasGroup canvasGroup;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Close();
    }
    public virtual void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void Reset()
    {
        if (TryGetComponent(out CanvasScaler canvasScaler))
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
    }
    [ContextMenu("Added Canvas Objects")]
    public void AddedCanvasObjectsAndSetProperties()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>() ?? GetComponent<Canvas>();
        CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>() ?? GetComponent<CanvasScaler>();
        if (!GetComponent<GraphicRaycaster>())
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }
#endif
}
