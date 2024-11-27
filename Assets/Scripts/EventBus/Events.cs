using prison161.EventBus;
using PuppetOfShadows.EventBinding;
using UnityEngine;

public interface IEvent { }

public struct HandleCharacterBalance : IEvent
{
    public float TargetRotationValue;
    public float RotationForce;
    public HandleCharacterBalance(float targetRotationValue, float rotationForce)
    {
        TargetRotationValue = targetRotationValue;

        RotationForce = rotationForce;
    }




}
public struct HandleDayNightCycle : IEvent
{
    
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