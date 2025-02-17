using UnityEngine;

public class DoorEvent
{
    public DoorTrigger door;
    public Transform targetPosition; // Oyuncunun gideceği nokta

    public DoorEvent(DoorTrigger door, Transform targetPosition)  //Constructor metod
    {
        this.door = door;
        this.targetPosition = targetPosition;
    }
}
