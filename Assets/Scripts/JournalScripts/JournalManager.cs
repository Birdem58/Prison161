using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class JournalManager : MonoBehaviour
{
    [Header("For JournalUI")]
    public Button[] bracketButtons;
    public GameObject[] panels;
    public GameObject jourCanvas;
  
    
    public GameObject journalIcon;
    public GameObject currentJournalIcon;
    public GameObject jourAlertUI;
    [Header("For Animation")]
    
    public float targetScale;
    public Transform targetPos;
 


    [Header("For Logic")]
    private bool isJournalOpened;
    public bool jourAlert;
    public bool visibDeneme;

    [Header("For Notes")]
    public TMP_InputField[] noteInputFields; 
    private Dictionary<int, string> savedNotes = new Dictionary<int, string>(); // Notlarý kaydedecek dictionary
    public AudioSource typeSoundEffect; // Yazý yazarken çalacak ses efekti

    [Header("For Dialog")]
    public GameObject journalIconPassive;
    private bool isInDialog;
    public int currentPerson;
    public Button passiveJournalButton;
    public Button closeJournalButton;


    void Start()
    {
        
        //Buttonlara journal  sayfalarýný açmamýzý saðlayacak olan dinleme kodu
        for (int i = 0; i < bracketButtons.Length;i++)
        {
            int index = i;
            bracketButtons[i].onClick.AddListener(() => OpenPanels(index));

        }

        
        InitializeNotes();
        InitializeJournalCanvas();
        SetJournalVisibility(1, true);
        SetJournalVisibility(0, true);
    }

 



    // Update is called once per frame
    void Update()
    {


        isInDialog = PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE;
        
        if (isInDialog)
        {
            JournalInDialog();
        }
        else
        {
            JournalOutDialog();
        }

        


        if (Input.GetKeyDown(KeyCode.Tab) && !isInDialog)
        {
            OpenJournalByTab();
         
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

   public void OpenJournalByTab()
        {
        Debug.Log("openjournalbytab");
        

        JournalAlert();

        if (!isJournalOpened)
        {
           
            if(isInDialog)
            {
                OpenPanels(currentPerson);
            }
            else
            { OpenPanels(0); }
            
            isJournalOpened = true;
           
            OpenJournalAnimation();
            passiveJournalButton.gameObject.SetActive(false);
            closeJournalButton.gameObject.SetActive(true);
        }
        else
        {
            CloseJournalAnimation();
            isJournalOpened = false;
            closeJournalButton.gameObject.SetActive(false);
            passiveJournalButton.gameObject.SetActive(true);
        }
    }
    void InitializeJournalCanvas()
    {
        jourCanvas.SetActive(false);
        journalIcon.SetActive(false);
        journalIconPassive.SetActive(false);
        currentJournalIcon = journalIcon;
        passiveJournalButton.onClick.AddListener(() => OpenJournalByTab());
        closeJournalButton.onClick.AddListener(() => CloseJournal());
        closeJournalButton.gameObject.SetActive(false);
       
    }

    void OpenJournalAnimation()
    {

      
        jourAlert = false;
        isJournalOpened = true;

        
       
            
         ShowJournalCanvas();
        


    }

    void CloseJournalAnimation()
    {
        isJournalOpened = false;

        HideJournalCanvas();
       

    }

    void ShowJournalCanvas()
    {
        jourCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PlayerState.Instance.SetCharacterController(false);
        currentJournalIcon.gameObject.SetActive(false);
    }

    void HideJournalCanvas()
    {
    jourCanvas.SetActive(false);
    currentJournalIcon.gameObject.SetActive(true); // Ýkonu tekrar göster

    
       if(!isInDialog)
        {
            PlayerState.Instance.SetState(PlayerState.State.DEFAULT);


        }
        
       
    }
    public void CloseJournal()
    {
        if (!isJournalOpened)
            return;

        CloseJournalAnimation();
        isJournalOpened = false;

        
        closeJournalButton.gameObject.SetActive(false);
        passiveJournalButton.gameObject.SetActive(true);
    }

    void JournalInDialog()
    {
        currentJournalIcon.gameObject.SetActive(false);
        currentJournalIcon = journalIconPassive;
        currentJournalIcon.gameObject.SetActive(true);
        currentJournalIcon = journalIconPassive.transform.GetChild(0).gameObject;
        currentJournalIcon.gameObject.SetActive(true);
        //burda ses oynatacak kod yazýlabilir

    }

    void JournalOutDialog()
    {

        currentJournalIcon.gameObject.SetActive(false);
        currentJournalIcon = journalIcon;
        currentJournalIcon.gameObject.SetActive(true);
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

    public void SetCurrentPerson(int index)
    {
        currentPerson = index;
        Debug.Log(currentPerson);
    }

    public void debugwatafak()
    {
        Debug.Log("butona basýldý");
    }
}



