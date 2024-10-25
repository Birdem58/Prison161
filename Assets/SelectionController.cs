using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{

    //singleton
    public static SelectionController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    [Serializable]
    public struct Selection
    {
        public GameManager.GameState Level;
        public int SelectionCount;
        public GameManager.GameState NextLevel;
    }
    public List<Selection> Selections;
    [SerializeField]GameObject SelectionCamera;

    //Selected Character list
    public List<SelectionCharacter> SelectedCharacters;
    public void StartSelection()
    {
        //Get the current level
        GameManager.GameState currentLevel = GameManager.Instance.State;

        //Find the selection for the current level
        Selection selection = Selections.Find(x => x.Level == currentLevel);

        SelectionCamera.SetActive(true);

        SelectedCharacters = null;

        
    }

    public void SelectCharacter(SelectionCharacter character)
    {
        //Get the current level
        GameManager.GameState currentLevel = GameManager.Instance.State;
        //Find the selection for the current level
        Selection selection = Selections.Find(x => x.Level == currentLevel);
        //Return if chracter count is more than the selection count
        if (SelectedCharacters != null && SelectedCharacters.Count >= selection.SelectionCount)
        {
            return;
        }

        if (SelectedCharacters == null)
        {
            SelectedCharacters = new List<SelectionCharacter>();
        }

        if (SelectedCharacters.Count < selection.SelectionCount)
        {
            SelectedCharacters.Add(character);
        }
    }
    public void EndSelection()
    {
        //Get the current level
        GameManager.GameState currentLevel = GameManager.Instance.State;
        //Find the selection for the current level
        Selection selection = Selections.Find(x => x.Level == currentLevel);

        SelectionCamera.SetActive(false);

        foreach (var character in SelectedCharacters)
        {
            character.GetComponent<CharacterLifeHandler>();
        }

        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
        GameManager.Instance.UpdateGameState(selection.NextLevel);
    }
}
