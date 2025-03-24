using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class SelectionMechanizm : MonoBehaviour
{
    
    public static SelectionMechanizm Instance { get; private set; }

    [Header("UI Elemanlarý")]
    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private Transform _listContent;
    [SerializeField] private GameObject _listItemPrefab;
    [SerializeField] private Button _confirmButton;

    [Header("Seçim Ayarlarý")]
    [SerializeField] private int _maxSelections = 5;
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeStrength = 0.2f;

    private List<string> _selectedCharacterNames = new List<string>();
    private List<GameObject> _instantiatedItems = new List<GameObject>();

    private SpriteRenderer _currentHoveredSprite;
    private Sprite _originalSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        _confirmButton.interactable = false;
        ToggleUI(false);
    }

    private void Update()
    {
        if (!_selectionUI.activeSelf) return;
        HandleRaycast();
        HandleSelectionInput();
    }

    private void HandleRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
        
            if (spriteRenderer != null && !_selectedCharacterNames.Contains(hit.collider.name))
            {
                OnHoverStart(spriteRenderer);
            }
            else
            {
                OnHoverEnd();
            }
        }
        else
        {
            OnHoverEnd();
        }
    }

    private void OnHoverStart(SpriteRenderer spriteRenderer)
    {
        if (_currentHoveredSprite != null) return;
        _currentHoveredSprite = spriteRenderer;
        _originalSprite = spriteRenderer.sprite;

       
        CharacterSprites charSprites = spriteRenderer.GetComponent<CharacterSprites>();
        if (charSprites != null)
        {
            spriteRenderer.sprite = charSprites.hoverSprite;
        }
        else
        {
          
            spriteRenderer.sprite = Resources.Load<Sprite>("HoverSprite");
        }

        transform.DOShakePosition(_shakeDuration, _shakeStrength);
    }

    private void OnHoverEnd()
    {
        if (_currentHoveredSprite == null) return;
       
        CharacterSprites charSprites = _currentHoveredSprite.GetComponent<CharacterSprites>();
        if (charSprites != null)
        {
            _currentHoveredSprite.sprite = charSprites.defaultSprite;
        }
        else
        {
            _currentHoveredSprite.sprite = _originalSprite;
        }
        _currentHoveredSprite = null;
    }

   
    private void HandleSelectionInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                string characterName = hit.collider.name;
                if (_selectedCharacterNames.Contains(characterName))
                {
                    RemoveSelection(characterName);
                }
                else if (_selectedCharacterNames.Count < _maxSelections)
                {
                    AddSelection(characterName, hit.collider.gameObject);
                }
                UpdateUI();
            }
        }
    }

   
    private void AddSelection(string characterName, GameObject characterObj)
    {
        _selectedCharacterNames.Add(characterName);

        CharacterSprites charSprites = characterObj.GetComponent<CharacterSprites>();
        if (charSprites != null)
        {
            SpriteRenderer sr = characterObj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = charSprites.selectedSprite;
            }
        }
    }

    
    private void RemoveSelection(string characterName)
    {
        _selectedCharacterNames.Remove(characterName);

      
        GameObject characterObj = GameObject.Find(characterName);
        if (characterObj != null)
        {
            CharacterSprites charSprites = characterObj.GetComponent<CharacterSprites>();
            if (charSprites != null)
            {
                SpriteRenderer sr = characterObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = charSprites.defaultSprite;
                }
            }
        }
    }

   
    private void UpdateUI()
    {
        foreach (var item in _instantiatedItems)
        {
            Destroy(item);
        }
        _instantiatedItems.Clear();

        foreach (var name in _selectedCharacterNames)
        {
            GameObject listItem = Instantiate(_listItemPrefab, _listContent);
            listItem.GetComponentInChildren<TMP_Text>().text = name;

           
            Button removeBtn = listItem.GetComponentInChildren<Button>();
            removeBtn.onClick.AddListener(() => RemoveSelection(name));

            _instantiatedItems.Add(listItem);
        }

        _counterText.text = $"{_selectedCharacterNames.Count}/{_maxSelections}";
        _confirmButton.interactable = (_selectedCharacterNames.Count == _maxSelections);
    }

   
    public void ToggleUI(bool state)
    {
        _selectionUI.SetActive(state);
    }

  
    public void ConfirmSelection()
    {
        if (_selectedCharacterNames.Count == _maxSelections)
        {
            Debug.Log("Seçim Tamamlandý!");
            ToggleUI(false);
        }
    }
}
