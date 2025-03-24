using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using prison161.EventBus;
public class SelectionConfirm : MonoBehaviour, IInteraction
{
    public string InteractionPrompt => "Confirm Selection [F]";
    public string signiture = "ḙ̸͖̬̘̭͈̯͚̝͍̘̜͚̌͒̈́̄̔́̾̉̇̾̄̎r̵͖͖̩̦̐͒̔̽̽̊̐́͛̌̈̂̂̕à̷̛͈̮̼͚̙͈͙́͑̒̅͘͜͠d̸̞͖͈̲̣͕̗̞̮̰̼͌̋̔̈́͝ͅé̸̢̳̪͕̠̓̈̽͊͠͝ͅç̶̻̃ͅȧ̸̪̘͇̳͚͌̅͝ͅͅt̵͔͉͍̿e̴̩͉̭͂͛̉̕d̷̘̖̳̞͎̯͎̥͂̓͊̔";
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private int fontSize;
    AudioSource audioSource;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }
    public virtual void Interact()
    {


        Signed();

    }
    public virtual void StopInteract()
    {
    
    }

   public void Signed()
    {
        textMeshPro.text = signiture;
        textMeshPro.fontSize = fontSize;
        audioSource.Play();
        EventBus<OnConfirmingSelection>.Raise(new OnConfirmingSelection("SelectionConfirmId"));
        Debug.Log(textMeshPro.text);

    }


}