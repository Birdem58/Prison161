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
        journalContent.alpha = 0; // Journal i�eri�i ba�ta gizli
        journalContent.interactable = false;
        journalContent.blocksRaycasts = false;
    }

    void Update()
    {
        
    }

    void OpenJournal()
    {
        isJournalOpen = true;

        // �konu b�y�tme ve ekran�n ortas�na ta��ma animasyonu
        journalIcon.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutQuad); // Ortaya ta��
        journalIcon.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.InOutQuad) // Boyut b�y�t
            .OnComplete(() =>
            {
                // Animasyon bitince journal panelini g�ster
                journalPanel.gameObject.SetActive(true);
                journalContent.DOFade(1, 0.3f); // ��eri�i yava��a g�ster
                journalContent.interactable = true;
                journalContent.blocksRaycasts = true;
            });
    }

    void CloseJournal()
    {
        isJournalOpen = false;

        // ��eri�i gizle
        journalContent.DOFade(0, 0.3f).OnComplete(() =>
        {
            journalPanel.gameObject.SetActive(false);
        });

        // �konu k���lt ve eski pozisyonuna d�nd�r
        journalIcon.DOAnchorPos(new Vector2(-Screen.width / 2 + 100, -Screen.height / 2 + 100), 0.5f).SetEase(Ease.InOutQuad);
        journalIcon.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
    }
}
