using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JournalManager : MonoBehaviour
{
    [Header("For JournalUI")]
    public Button[] bracketButtons;
    public GameObject[] panels;
    public GameObject jourCanvas;
  
    public GameObject jourCanvasPassive;
    public RectTransform journalIcon;
    
    public GameObject jourAlertUI;
    [Header("For Animation")]
    public float animDuration;
    public float targetScale;
    public Transform targetPos;
    private Vector3 iconStartPos;
    private bool isAnimating;
    [Header("For Logic")]
    private bool isJournalOpened;
    public bool jourAlert;
    public bool journalvisibiliytyDEmee;



    void Start()
    { 

        iconStartPos = journalIcon.transform.position;
        for (int i = 0; i < bracketButtons.Length;i++)
        {
            int index = i;
            bracketButtons[i].onClick.AddListener(() => OpenPanels(index));

        }

        SetJournalVisibility(0,true);
        SetJournalVisibility(1, true);
     
        InitializeJournalCanvas();

    }

 



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OpenJournalByTab();
            JournalAlert();
           
            
        }     
    }

    

    void OpenPanels(int index)
    {
        if (!panels[index].activeSelf)
            return;

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[index].SetActive(true);
        Debug.Log(index);
    }

    void OpenJournalByTab()
    {
        if (isAnimating) return; 

        if (!isJournalOpened)
        {
            OpenPanels(1);
            isJournalOpened = true;
            PlayerState.Instance.SetState(PlayerState.State.NONE);
            OpenJournalAnimation();
        }
        else
        {
            CloseJournalAnimation();
            isJournalOpened = false;
        }
    }
    void InitializeJournalCanvas()
    {
        jourCanvas.SetActive(false);
    }

    void OpenJournalAnimation()
    {
        jourAlert = false;
        isJournalOpened = true;
        PlayerState.Instance.SetState(PlayerState.State.NONE);
        // journalIcon.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutQuad);
        // journalIcon.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.InOutQuad).OnComplete(ShowJournalCanvas);
       
            isAnimating = true; 
            Sequence openingSequence = DOTween.Sequence();
            openingSequence.Append(journalIcon.transform.DOScale(targetScale, animDuration))
                      .Join(journalIcon.transform.DOMove(targetPos.transform.position, animDuration)
                      .SetEase(Ease.InOutQuad))
                      .OnComplete(() =>
                      {
                          ShowJournalCanvas();
                          isAnimating = false; 
                      });
            openingSequence.Play();
        
    }

    void CloseJournalAnimation()
    {
        isJournalOpened = false;

     
            //journalIcon.DOAnchorPos(new Vector2(-Screen.width / 2 + 100, -Screen.height / 2 + 100), 0.5f).SetEase(Ease.InOutQuad);
            //journalIcon.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
        
        isAnimating = true; 
        HideJournalCanvas(() =>
        {
            journalIcon.transform.DOScale(Vector3.one, animDuration);
            journalIcon.transform.DOMove(iconStartPos, animDuration).OnComplete(() =>
            {
                isAnimating = false; 
                PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
            });
        });
    }

    void ShowJournalCanvas()
    {
        jourCanvas.SetActive(true);
        journalIcon.gameObject.SetActive(false);
    }

    void HideJournalCanvas(TweenCallback onComplete = null)
    {
        jourCanvas.SetActive(false);
    jourCanvasPassive.SetActive(false);
    journalIcon.gameObject.SetActive(true); // Ýkonu tekrar göster
    PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
    onComplete?.Invoke();
       
    }
   
    void JournalInDialogPassive()
    {
        jourCanvas.SetActive(false);
        jourCanvasPassive.SetActive(true);
        //burda ses oynatacak kod yazýlabilir

    }

    void JournalInDialogActive()
    {

        jourCanvasPassive.SetActive(false);
        jourCanvas.SetActive(true);
        //burda ses oynatacak kod yazýlabilir
    }

    void JournalAlert()
    {
        if(jourAlert)
        {
            jourAlertUI.SetActive(true);

        }
        else
        {
            jourAlertUI.SetActive(false);
        }
    }

    public void SetJournalVisibility(int pageIndex, bool isVisible)
    {
        
        if (pageIndex >= 0 && pageIndex < panels.Length)
        {
            panels[pageIndex].SetActive(isVisible);
        }
        
        if (pageIndex >= 0 && pageIndex < bracketButtons.Length)
        {
            bracketButtons[pageIndex].gameObject.SetActive(isVisible);
        }

        if (isVisible)
        {
            Debug.Log($" page and button opened{pageIndex}");
        }
        else
        {
            Debug.Log($"page and Button{pageIndex} closed");
        }
    }
}



