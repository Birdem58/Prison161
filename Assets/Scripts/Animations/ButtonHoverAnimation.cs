using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class ButtonHoverAnimation : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;    
    private RectTransform originalRectTransform;
    private Sequence hoverSeq;

    [SerializeField] private float moveDistance = 10f; 

    [SerializeField] private float moveDuration = 0.2f;


    private void Start()
    {
         
        rectTransform = GetComponent<RectTransform>();
        originalRectTransform = rectTransform;  
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        hoverSeq.Kill(true);
        hoverSeq = DOTween.Sequence();
        hoverSeq.Join(rectTransform.DOMoveX(rectTransform.position.x + moveDistance, moveDuration).SetEase(Ease.OutBack));
        
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverSeq.Kill(true);
        hoverSeq = DOTween.Sequence();
        hoverSeq.Join(rectTransform.DOMoveX(rectTransform.position.x - moveDistance, moveDuration).SetEase(Ease.OutBack));
    }
}   

