using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;

public class JournalManager : MonoBehaviour
{
    [System.Serializable]
    public class JournalPage
    {
        public string pageId; 
        public GameObject pageObject;
        public TMP_InputField noteInputField;
        public bool hasNoteInput = true;
    }

    [System.Serializable]
    public class JournalCategory
    {
        public string categoryName;
        public Button categoryButton;
        public GameObject categoryPanel;
        public List<JournalPage> pages = new List<JournalPage>();
        public List<Button> pageBracketButtons = new List<Button>();
    }

    [Header("Journal Categories")]
    public List<JournalCategory> journalCategories = new List<JournalCategory>();

    [Header("UI References")]
    public GameObject jourCanvas;
    public GameObject journalIcon;
    public GameObject jourAlertUI;
    
    public GameObject backButton;

    [Header("Settings")]
    public float animationDuration = 0.3f;
    public AudioSource typeSoundEffect;

    private Dictionary<string, string> savedNotes = new Dictionary<string, string>();
    private int currentCategoryIndex = -1;
    private int currentPageIndex = -1;
    private bool isInCategoryView = true;
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
        LoadSavedNotes();
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
        backButton.SetActive(false);

        // Setup category buttons
        for (int i = 0; i < journalCategories.Count; i++)
        {
            int categoryIndex = i;
            JournalCategory category = journalCategories[i];
            
            // Setup category button
            if (category.categoryButton != null)
            {
                category.categoryButton.onClick.AddListener(() => OpenCategory(categoryIndex));
            }
            
            // Setup panel
            if (category.categoryPanel != null)
            {
                category.categoryPanel.SetActive(false);
            }
            
            // Connect bracket buttons to pages
            int minLength = Mathf.Min(category.pageBracketButtons.Count, category.pages.Count);
            for (int j = 0; j < minLength; j++)
            {
                int pageIndex = j;
                Button bracketButton = category.pageBracketButtons[j];
                
                if (bracketButton != null)
                {
                    bracketButton.onClick.RemoveAllListeners(); // Clear any existing listeners
                    bracketButton.onClick.AddListener(() => OpenPage(categoryIndex, pageIndex));
                }
                
                // Make sure page objects are initially hidden
                if (category.pages[j].pageObject != null)
                {
                    category.pages[j].pageObject.SetActive(false);
                }
                
                // Setup input field change listener
                JournalPage page = category.pages[j];
                if (page.hasNoteInput && page.noteInputField != null)
                {
                    TMP_InputField inputField = page.noteInputField;
                    inputField.onValueChanged.RemoveAllListeners();
                    inputField.onValueChanged.AddListener((text) => SaveNote(categoryIndex, pageIndex, text));
                }
            }
        }

        // Setup back button
        Button backButtonComponent = backButton.GetComponent<Button>();
        if (backButtonComponent != null)
        {
            backButtonComponent.onClick.RemoveAllListeners();
            backButtonComponent.onClick.AddListener(GoBack);
        }
        
        ShowCategoryView();
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
        
        if (isJournalOpened && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isInCategoryView)
            {
                GoBack();
            }
            else
            {
                ToggleJournal();
            }
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
            ShowCategoryView();
        }
    }

    public void OpenCategory(int categoryIndex)
    {
        if (categoryIndex < 0 || categoryIndex >= journalCategories.Count)
            return;
            
        currentCategoryIndex = categoryIndex;
        isInCategoryView = false;
        
        // Hide all category panels and show only the selected one
        for (int i = 0; i < journalCategories.Count; i++)
        {
            if (journalCategories[i].categoryPanel != null)
            {
                journalCategories[i].categoryPanel.SetActive(i == categoryIndex);
            }
        }
        
        

        
      
    }

    public void OpenPage(int categoryIndex, int pageIndex)
    {
        if (categoryIndex < 0 || categoryIndex >= journalCategories.Count)
            return;
            
        JournalCategory category = journalCategories[categoryIndex];
        
        if (pageIndex < 0 || pageIndex >= category.pages.Count)
            return;
            
        // Hide all pages in this category
        for (int i = 0; i < category.pages.Count; i++)
        {
            if (category.pages[i].pageObject != null)
            {
                category.pages[i].pageObject.SetActive(i == pageIndex);
            }
        }
        
        currentCategoryIndex = categoryIndex;
        currentPageIndex = pageIndex;
        
        // Load note if this page has an input field
        JournalPage currentPage = category.pages[pageIndex];
        if (currentPage.hasNoteInput && currentPage.noteInputField != null)
        {
            string noteKey = GetNoteKey(currentPage);
            if (savedNotes.ContainsKey(noteKey))
            {
                currentPage.noteInputField.text = savedNotes[noteKey];
            }
        }
    }

    public void GoBack()
    {
        ShowCategoryView();
    }

    private void ShowCategoryView()
    {
        isInCategoryView = true;
        currentPageIndex = -1;
        
        // Hide all category panels
        for (int i = 0; i < journalCategories.Count; i++)
        {
            if (journalCategories[i].categoryPanel != null)
            {
                journalCategories[i].categoryPanel.SetActive(false);
            }
        }
        
        
    }

    private string GetNoteKey(JournalPage page)
    {
        // Use the pageId if available, otherwise fallback to indices
        if (!string.IsNullOrEmpty(page.pageId))
        {
            return page.pageId;
        }
        
        // Fallback
        return $"cat_{currentCategoryIndex}_page_{currentPageIndex}";
    }

    public void SaveNote(int categoryIndex, int pageIndex, string text)
    {
        if (categoryIndex < 0 || categoryIndex >= journalCategories.Count)
            return;
            
        JournalCategory category = journalCategories[categoryIndex];
        
        if (pageIndex < 0 || pageIndex >= category.pages.Count)
            return;
            
        JournalPage page = category.pages[pageIndex];
        
        if (!page.hasNoteInput || string.IsNullOrEmpty(text))
            return;
            
        string noteKey = !string.IsNullOrEmpty(page.pageId) ? page.pageId : $"cat_{categoryIndex}_page_{pageIndex}";
        
        // Update or add the note in the dictionary
        if (savedNotes.ContainsKey(noteKey))
        {
            savedNotes[noteKey] = text;
        }
        else
        {
            savedNotes.Add(noteKey, text);
        }
        
        // Play typing sound
        if (typeSoundEffect != null)
        {
            typeSoundEffect.Play();
        }
        
        // Save to PlayerPrefs for persistence
        PlayerPrefs.SetString("JournalNote_" + noteKey, text);
        PlayerPrefs.Save();
    }

    private void LoadSavedNotes()
    {
        savedNotes.Clear();
        
        // Load all notes from PlayerPrefs
        foreach (var category in journalCategories)
        {
            foreach (var page in category.pages)
            {
                if (page.hasNoteInput)
                {
                    string noteKey = !string.IsNullOrEmpty(page.pageId) ? page.pageId : "unknown_page";
                    string savedText = PlayerPrefs.GetString("JournalNote_" + noteKey, "");
                    
                    if (!string.IsNullOrEmpty(savedText))
                    {
                        savedNotes[noteKey] = savedText;
                        
                        // If this page is currently visible, update its input field
                        if (page.noteInputField != null)
                        {
                            page.noteInputField.text = savedText;
                        }
                    }
                }
            }
        }
    }

    public void ToggleJournalAlert(bool state) => jourAlertUI.SetActive(state);
    
    public void ToggleCategoryVisibility(int categoryIndex, bool state)
    {
        if (categoryIndex < 0 || categoryIndex >= journalCategories.Count)
            return;
            
        if (journalCategories[categoryIndex].categoryButton != null)
        {
            journalCategories[categoryIndex].categoryButton.gameObject.SetActive(state);
        }
    }
    
    public void TogglePageBracketVisibility(int categoryIndex, int bracketIndex, bool state)
    {
        if (categoryIndex < 0 || categoryIndex >= journalCategories.Count)
            return;
            
        JournalCategory category = journalCategories[categoryIndex];
        
        if (bracketIndex < 0 || bracketIndex >= category.pageBracketButtons.Count)
            return;
            
        if (category.pageBracketButtons[bracketIndex] != null)
        {
            category.pageBracketButtons[bracketIndex].gameObject.SetActive(state);
        }
    }
}