using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;

public class JournalManager : MonoBehaviour
{
    [Header("UI References")]
    public Button[] bracketButtons;
    public GameObject[] panels;
    public GameObject jourCanvas;
    public GameObject journalIcon; 
    public GameObject jourAlertUI;
    public TMP_InputField[] noteInputFields;

    [Header("Settings")]
    public float animationDuration = 0.3f;
    public AudioSource typeSoundEffect;

    private Dictionary<int, string> savedNotes = new Dictionary<int, string>();
    private int currentPanelIndex = -1;
    private int currentPerson;
    private bool isJournalOpened;
    private bool isInDialog;

  
    private bool canOpenJournal = false;
    private EventBinding<GetJournal> journalEventBinding;   

    private void OnEnable()
    {
        journalEventBinding = new EventBinding<GetJournal>(OnInitilaizeJournal);
        EventBus<GetJournal>.Register(journalEventBinding); 
    }

    private void OnDisable()
    {
         EventBus<GetJournal>.Deregister(journalEventBinding); 
    }

    void Update()
    {
        HandlePlayerState();
        HandleJournalInput();
    }

    private void Start()
    {
        
        canOpenJournal = false;
       
        InitializeUI();
        LoadNotes();
    }
    private void OnInitilaizeJournal(GetJournal getJournal)
    {
        if (getJournal.journalEnable)
        {
            canOpenJournal = getJournal.journalEnable; 
            if (typeSoundEffect != null)
            {
                typeSoundEffect.Play();
            }

        }
    }

    private void InitializeUI()
    {
        
        jourCanvas.SetActive(false);
        
        journalIcon.SetActive(false);

        for (int i = 0; i < bracketButtons.Length; i++)
        {
            int index = i;
            bracketButtons[i].onClick.AddListener(() => TogglePanel(index));
        }

        TogglePanel(0);
    }

    private void HandlePlayerState()
    {
        isInDialog = PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE;
        UpdateJournalIcon();
    }

    
    private void UpdateJournalIcon()
    {
        journalIcon.SetActive(canOpenJournal && !isJournalOpened && !isInDialog);
    }

    private void HandleJournalInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleJournal();
        }
    }

    public void ToggleJournal()
    {
     
        if (!canOpenJournal)
            return;

        isJournalOpened = !isJournalOpened;
        UpdateJournalState();
    }

    
    private void UpdateJournalState()
    {
        jourCanvas.SetActive(isJournalOpened);
        Cursor.lockState = isJournalOpened ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isJournalOpened;
        PlayerState.Instance.SetCharacterController(!isJournalOpened);

       
        journalIcon.SetActive(!isJournalOpened);

        if (isJournalOpened)
        {
            TogglePanel(isInDialog ? currentPerson : 0);
        }
    }

    public void TogglePanel(int index)
    {
        if (currentPanelIndex == index) return;

        if (currentPanelIndex >= 0)
        {
            panels[currentPanelIndex].SetActive(false);
        }

        currentPanelIndex = index;
        panels[index].SetActive(true);
        UpdateNoteDisplay(index);
    }

    private void UpdateNoteDisplay(int index)
    {
        if (index < noteInputFields.Length)
        {
            noteInputFields[index].text = savedNotes.ContainsKey(index)
                ? savedNotes[index]
                : string.Empty;
        }
    }

    private void LoadNotes()
    {
        for (int i = 0; i < noteInputFields.Length; i++)
        {
            if (savedNotes.ContainsKey(i))
            {
                noteInputFields[i].text = savedNotes[i];
            }
        }
    }

    public void OnNoteChanged(int pageIndex)
    {
        if (pageIndex >= noteInputFields.Length) return;

        string note = noteInputFields[pageIndex].text;
        if (string.IsNullOrEmpty(note)) return;

        if (savedNotes.ContainsKey(pageIndex))
        {
            savedNotes[pageIndex] = note;
        }
        else
        {
            savedNotes.Add(pageIndex, note);
        }

        if (typeSoundEffect != null)
        {
            typeSoundEffect.Play();
        }
    }

    public void SetCurrentPerson(int index) => currentPerson = index;

   
    public void ToggleJournalAlert(bool state) => jourAlertUI.SetActive(state);
    public void ToggleJournalElements(int index, bool state)
    {
        if (index >= 0 && index < panels.Length) panels[index].SetActive(state);
        if (index >= 0 && index < bracketButtons.Length) bracketButtons[index].gameObject.SetActive(state);
    }
}
