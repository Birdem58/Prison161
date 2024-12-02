using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    public Button[] bracketButtons;
    public GameObject[] panels;
    public GameObject journalCanvas;
    public RectTransform journalTransform;
    public RectTransform tabPositon;
    public RectTransform dialogPosClosed;
    public RectTransform dialogPosOpened;
    void Start()
    {
        for (int i = 0; i <=bracketButtons.Length;i++)
        {
            int index = i;
            bracketButtons[i].onClick.AddListener(() =>  OpenPanels(index));
        
        
        
        }
        
    }

    void OpenPanels(int index)
    {
        for(int i= 0; i <= panels.Length;i++) 
        {
            panels[i].SetActive(false);
        }
        panels[index].SetActive(true);
    }

    void OpenJournalByTab()
    {
        journalCanvas.SetActive(true);
        journalTransform.position = tabPositon.position;
        
    }

    void JournalInDialogPassive()
    {
        journalCanvas.SetActive(true);
        journalTransform.position = dialogPosClosed.position;
    }

    void JournalInDialogActive()
    {
        journalCanvas.SetActive(true);
        journalTransform.position = dialogPosOpened.position;
    }



    
    // Update is called once per frame
    void Update()
    {
        
    }
}
