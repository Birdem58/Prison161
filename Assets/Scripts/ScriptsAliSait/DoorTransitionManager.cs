using UnityEngine;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;

public class DoorTransitionManager : MonoBehaviour
{
    private void OnEnable()
    {

        EventBus<DoorEvent>.Register(new EventBinding<DoorEvent>(OnDoorUsed));
        
    }

    private void OnDisable()
    {

        EventBus<DoorEvent>.Deregister(new EventBinding<DoorEvent>(OnDoorUsed));
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
