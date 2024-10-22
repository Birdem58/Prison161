using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VIDE_Data;

public class VıdePlayer : MonoBehaviour
{
    //This script handles player movement and interaction with other NPC game objects

    public string playerName = "VIDE User";
    public List<GraphicRaycaster> raycasters;
    public EventSystem eventSystem;

    //Reference to our diagUI script for quick access
    public VIDEUIManager4 diagUI;
    //public QuestChartDemo questUI;
    //public Animator blue;

    //Stored current VA when inside a trigger
    public VIDE_Assign videAssign;

    //DEMO variables for item inventory
    //Crazy cap NPC in the demo has items you can collect
    public List<string> demo_Items = new List<string>();
    public List<string> demo_ItemInventory = new List<string>();

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VIDE_Assign>() != null)
            videAssign = other.GetComponent<VIDE_Assign>();
    }

    void OnTriggerExit()
    {
        videAssign = null;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.F))
        {
            if (videAssign != null)
            {

                TryInteract();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();

            // Loop through each raycaster and perform raycast
            foreach (var raycaster in raycasters)
            {
                raycaster.Raycast(pointerEventData, raycastResults);
            }

            if (raycastResults.Count > 0)
            {
                GameObject hitUIElement = raycastResults[0].gameObject;

                videAssign = hitUIElement.GetComponent<VIDE_Assign>();

                if (videAssign != null)
                {
                    Debug.Log("Component found on UI element: " + videAssign.name);
                     TryInteract();
                }
                else
                {
                    Debug.Log("VIDE_Assign component not found on UI element.");
                }
            }
        }
        

        
    }


    void TryInteract()
    {
       

        if (videAssign)
        {
            
            diagUI.Interact(videAssign);
          
            return;
        }


    }
}
