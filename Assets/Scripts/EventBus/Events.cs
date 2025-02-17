using prison161.EventBus;
using PuppetOfShadows.EventBinding;
using UnityEngine;

public interface IEvent { }



public struct GetJournal : IEvent
{
    public bool journalEnable;
    public GetJournal(bool enable)
    {
        journalEnable = enable;
    }
}

public struct DoorEvent: IEvent
{
    public DoorTrigger door;
    public Transform targetPosition; // Oyuncunun gideceði nokta

    public DoorEvent(DoorTrigger door, Transform targetPosition)  //Constructor metod
    {
        this.door = door;
        this.targetPosition = targetPosition;
    }
}





//örnek kullaným:
//using Prison161.EventBus;
//using Prison161.EventBinding;

//public class BodyBalance : MonoBehaviour
//{
//    public float targetRotation;
//    Rigidbody2D rb;
//    public float force;
//    EventBinding<HandleCharacterBalance> balanceBinding;
//    // Start is called before the first frame update
//    void Start()
//    {
//        balanceBinding = new EventBinding<HandleCharacterBalance>(OnBalanceChange);
//        EventBus<HandleCharacterBalance>.Register(balanceBinding);
//        rb = gameObject.GetComponent<Rigidbody2D>();
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.deltaTime));
//    }
//    private void OnDestroy()
//    {
//        EventBus<HandleCharacterBalance>.Deregister(balanceBinding);
//    }

//    void OnBalanceChange(HandleCharacterBalance handleCharacterBalance)
//    {
//        targetRotation = handleCharacterBalance.TargetRotationValue;
//        force = handleCharacterBalance.RotationForce;
//        //aaaa

//    }
//}