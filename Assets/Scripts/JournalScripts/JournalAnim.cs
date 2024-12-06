using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JournalAnim: MonoBehaviour
{
    public RectTransform journalIcon;  
    public RectTransform journalPanel; 
    public CanvasGroup journalContent; 

    private bool isJournalOpen = false;

    void Start()
    {
        journalContent.alpha = 0; // Journal içeriði baþta gizli
        journalContent.interactable = false;
        journalContent.blocksRaycasts = false;
    }

    void Update()
    {
        
    }

    void OpenJournal()
    {
        isJournalOpen = true;

        // Ýkonu büyütme ve ekranýn ortasýna taþýma animasyonu
        journalIcon.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutQuad); // Ortaya taþý
        journalIcon.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.InOutQuad) // Boyut büyüt
            .OnComplete(() =>
            {
                // Animasyon bitince journal panelini göster
                journalPanel.gameObject.SetActive(true);
                journalContent.DOFade(1, 0.3f); // Ýçeriði yavaþça göster
                journalContent.interactable = true;
                journalContent.blocksRaycasts = true;
            });
    }

    void CloseJournal()
    {
        isJournalOpen = false;

        // Ýçeriði gizle
        journalContent.DOFade(0, 0.3f).OnComplete(() =>
        {
            journalPanel.gameObject.SetActive(false);
        });

        // Ýkonu küçült ve eski pozisyonuna döndür
        journalIcon.DOAnchorPos(new Vector2(-Screen.width / 2 + 100, -Screen.height / 2 + 100), 0.5f).SetEase(Ease.InOutQuad);
        journalIcon.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
    }
}
