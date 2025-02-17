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
    [Tooltip("FaxPaperlarý yaz")]
    public List<string> faxDescriptions;

    [Header("Fax Papers")]
    [Tooltip("FaxPaperlarý belirle")]
    public  List<GameObject> paperList;
    public int faxIndex = 0;

    [Header("TextOverlay")]
    public GameObject backgroundImg;
    public TextMeshProUGUI faxDescText;

    private bool isTriggered = false;
    public Animator animator;

    //bir tane Buttonumuz olacak ona onClick dedikten sonra
    //burdan bir metodu çalýþtýracaðýz o metod ise hafif karanlýk bir resim üstüne fax descriptionu yazacak
    //birde kapama iþine yarayacak buton olucak o da textoverlayi gizleyecek imagý bir container olarak düþünürsek sadece imagý açýp kapatmamýz lazým


    //yapýlacaklar Buton ekleyip fonksyonlarý denemek daha sonra trigerlamak ve objective systemiyle uyumluluðunu saðlamak daha daha sonra gamemanagerla indexleri arttýrýp farklý faxlarýn çýkmasýný saðlamak
    void ShowTextOverlay()
    {
        faxDescText.text = faxDescriptions[faxIndex];
        backgroundImg.SetActive(true);
        
    }

    void HideTextOverlay() { backgroundImg.SetActive(false);
    }

    // game managerda her bir state geldiðinde ayrý bir geçiþ yapabiliriz !!Bu mantýklý

    

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
