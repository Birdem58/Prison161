using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDayManager : MonoBehaviour
{
    public GameObject roomDoor;
    public GameObject anna;
    public GameObject evelyn;
    public GameObject caleb;
    public GameObject annaOpRoom;
    public GameObject evelynOpRoom;
    public GameObject calebOpRoom;
    public bool isAcsessOpRoom;
    public int talkedPeople = 0 ;
    public GameObject blockToOpRoom;
    
    
    
    private void Awake()
    {
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
    }
    private void GameManager_OnGameStateChange(GameManager.GameState state)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(talkedPeople == 3) 
        {
          isAcsessOpRoom = true;
          blockToOpRoom.SetActive(false);
          Debug.Log("Accses granterd");
        }
    }

    public void IncrementTalkedPeople(int howmuch)
    {
        if(talkedPeople<3)
        { talkedPeople+= howmuch; }
        
    }
   
   public void MoveRoomDoor()
    {
        //roomDoor.transform.position += new Vector3(0,0,-2.05f);
    }
}
