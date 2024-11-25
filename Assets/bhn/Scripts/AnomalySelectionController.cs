using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalySelectionController : MonoBehaviour
{
    //public AnomalySelectionManager anomalySelectionManager;
    
    public Color highlightColor=Color.yellow;
    private Color _originalColor;
    private Renderer _charRenderer;

    private bool _hasClicked;
    
    
    private void Awake()
    {
        _charRenderer = GetComponent<Renderer>();
        _hasClicked = false;
    }

    private void Start()
    {
        _charRenderer = GetComponent<Renderer>();
        _originalColor = _charRenderer.material.color;
    }


    private void OnMouseDown()
    {
        if (!_hasClicked)
        {
            HighlightChar();
        }


        else if (_hasClicked)
        {
            ResetHighlight();
        }
        
        AnomalySelectionManager.Instance.UpdateSelection(this.tag, _hasClicked);
            
    }

    public void HighlightChar()
    {
        _charRenderer.material.color = highlightColor;
        _hasClicked = true;
    }

    public void ResetHighlight()
    {
        _charRenderer.material.color = _originalColor;
        _hasClicked = false;
    }
}
