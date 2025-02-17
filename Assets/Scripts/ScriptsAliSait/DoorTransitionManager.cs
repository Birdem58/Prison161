using UnityEngine;

public class DoorTransitionManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus<DoorEvent>.Subscribe(OnDoorUsed);
    }

    private void OnDisable()
    {
        EventBus<DoorEvent>.Unsubscribe(OnDoorUsed);
    }

    private void OnDoorUsed(DoorEvent doorEvent)
    {
        Debug.Log("Kapıdan geçiliyor... Yeni hedef: " + doorEvent.targetPosition.name);

        // Oyuncuyu yeni konuma taşı
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = doorEvent.targetPosition.position;

        Debug.Log("Oyuncu yeni konuma taşındı: " + player.position);
    }
}
