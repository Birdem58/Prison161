using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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
    public bool visibDeneme;

    [Header("For Notes")]
    public TMP_InputField[] noteInputFields; 
    private Dictionary<int, string> savedNotes = new Dictionary<int, string>(); // Notlarý kaydedecek dictionary
    public AudioSource typeSoundEffect; // Yazý yazarken çalacak ses efekti



    void Start()
    { 

        iconStartPos = journalIcon.transform.position;
        //Buttonlara journal  sayfalarýný açmamýzý saðlayacak olan dinleme kodu
        for (int i = 0; i < bracketButtons.Length;i++)
        {
            int index = i;
            bracketButtons[i].onClick.AddListener(() => OpenPanels(index));

        }


        InitializeNotes();
        InitializeJournalCanvas();
        SetJournalVisibility(1, false);
        SetJournalVisibility(0, true);
    }

 



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OpenJournalByTab();
            JournalAlert();
        }     
        if(Input.GetKeyDown(KeyCode.G))
        {
            if (!visibDeneme)
            {
                SetJournalVisibility(1, true);
                visibDeneme = true;
            }
        
        }
    }



    void OpenPanels(int index)
    {
        OnNoteChanged(index);
        //journalýn panellerini kapatýyoruz ve daha sonra asýl paneli tekrardan aktif hale getiriyoruz
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[index].SetActive(true);
        Debug.Log(index);
        if (noteInputFields.Length > index)
        {
            if(savedNotes.ContainsKey(index))
            {
                noteInputFields[index].text = savedNotes[index];
            }
            else
            {
                noteInputFields[index].text = "";
            }
            noteInputFields[index].text = savedNotes.ContainsKey(index) ? savedNotes[index] : "";
        }

    }

    void OpenJournalByTab()
    {
        if (isAnimating) 
            return; 

        if (!isJournalOpened)
        {
           
            
            OpenPanels(0);
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


    private void InitializeNotes()
    {
        for(int i = 0; i < noteInputFields.Length;i++)
        {
            if (savedNotes.ContainsKey(i))
            {
                noteInputFields[i].text = savedNotes[i];
            }
        }
    }
    private void PlayTypingSoundEffect()
    {
        if (typeSoundEffect != null)
        {

            typeSoundEffect.Play();
        }
    }
    public void OnNoteChanged(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < noteInputFields.Length)
        {
            string note = noteInputFields[pageIndex].text;
            if (savedNotes.ContainsKey(pageIndex))
            {
                savedNotes[pageIndex] = note;
            }
            else
            {
                savedNotes.Add(pageIndex, note);
            }
            PlayTypingSoundEffect();
        }
    }
}



