using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FaxInteraction : MonoBehaviour, IInteraction
{
    public GameObject wallCollider;
    public string InteractionPrompt => "Todays Fax Paper[F]";
    [Header("Fax Descriptions")]
    [Tooltip("FaxPaperlar� yaz")]
    public List<string> faxDescriptions;

    [Header("Fax Papers")]
    [Tooltip("FaxPaperlar� belirle")]
    public  List<GameObject> paperList;
    public int faxIndex = 0;

    [Header("TextOverlay")]
    public GameObject backgroundImg;
    public TextMeshProUGUI faxDescText;

    private bool isTriggered = false;
    public Animator animator;

    //bir tane Buttonumuz olacak ona onClick dedikten sonra
    //burdan bir metodu �al��t�raca��z o metod ise hafif karanl�k bir resim �st�ne fax descriptionu yazacak
    //birde kapama i�ine yarayacak buton olucak o da textoverlayi gizleyecek imag� bir container olarak d���n�rsek sadece imag� a��p kapatmam�z laz�m


    //yap�lacaklar Buton ekleyip fonksyonlar� denemek daha sonra trigerlamak ve objective systemiyle uyumlulu�unu sa�lamak daha daha sonra gamemanagerla indexleri artt�r�p farkl� faxlar�n ��kmas�n� sa�lamak
    void ShowTextOverlay()
    {
        faxDescText.text = faxDescriptions[faxIndex];
        backgroundImg.SetActive(true);
        
    }

    void HideTextOverlay() { backgroundImg.SetActive(false);
    }

    // game managerda her bir state geldi�inde ayr� bir ge�i� yapabiliriz !!Bu mant�kl�

    

    void PickUpPaper()
    {
        PlayerState.Instance.SetState(PlayerState.State.DIALOGUE);
        paperList[faxIndex].SetActive(true);   
        wallCollider.SetActive(false);
        animator.SetInteger("doorPos",1);
    }

    public void DropPaper()
    {
        paperList[faxIndex].SetActive(false);
        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
    }


    public void Interact()
    {
        if(!isTriggered){
            isTriggered = true;
            PickUpPaper();
        }else StopInteract();
    }

    public void StopInteract()
    {
        isTriggered = false;
        DropPaper();
    }
}
