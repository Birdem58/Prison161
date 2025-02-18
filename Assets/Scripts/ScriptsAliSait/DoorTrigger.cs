using UnityEngine;
using System.Collections;
using prison161.EventBus;


public class DoorTrigger : MonoBehaviour, IInteraction
{

    public string InteractionPrompt => "Interact[F]";
    [SerializeField] private Collider insideArea; // Kapının iç tarafındaki geniş alan (BoxCollider)
    [SerializeField] private Collider outsideArea; // Kapının dış tarafındaki geniş alan (BoxCollider)
    [SerializeField] private float transitionDelay = 1.5f;


    private bool canInteract = false;
    private bool isTransitioning = false; // Geçiş yapılıp yapılmadığını kontrol eder
    private bool isInside = false; // Oyuncunun içeride mi dışarıda mı olduğunu takip eder
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            DeterminePlayerPosition(other.transform);
        }
    }
    public void Interact()
    {
        Debug.Log(canInteract);
        if (canInteract && !isTransitioning)
        {
            Debug.Log("F tuşuna basıldı! Kapıdan geçiş başlıyor.");
            StartCoroutine(HandleDoorTransition());
        }
    }
    public void StopInteract()
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;    //kapıdan uzaklaşıınca giriş yapama
        }
    }

    private void DeterminePlayerPosition(Transform player)
    {
        if (insideArea.bounds.Contains(player.position))  //oyuncunun pozisyonu insideArea içerisindeyse
        {
            isInside = true;
        }
        else if (outsideArea.bounds.Contains(player.position))
        {
            isInside = false;
        }
    }

    private void Update()
    {
      
    }

    private IEnumerator HandleDoorTransition()  //IEnumearor sayesinde daha yumuşak geçişler sağlıyoruz , yield return ile oyunu dondurmadan bekleme yapabiliyoruz.
    {
        isTransitioning = true; // Yeni girişleri engelle

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;  //?. sayesinde Player'ın null kontrolü yapılıyor

        if (player == null)
        {
            Debug.LogError("HATA: Oyuncu bulunamadı! Oyuncunun 'Player' etiketi olduğundan emin olun.");
            yield break;
        }

        // Oyuncunun hangi tarafa geçeceğini belirle
        Transform targetPoint = isInside ? outsideArea.transform : insideArea.transform;


        if (targetPoint == null)
        {
            Debug.LogError("HATA: InsidePoint veya OutsidePoint eksik! Lütfen Inspector'dan kontrol edin.");
            yield break;
        }

        Debug.Log($"F tuşuna basıldı! Oyuncunun şu anki konumu: {player.position}");
        Debug.Log($"Oyuncu şu noktaya gidecek: {targetPoint.position}");

        // FADE OUT (Ekranı karart)
        EventBus<DoorEvent>.Raise((new DoorEvent(this, targetPoint)));
        yield return new WaitForSeconds(transitionDelay / 2);

        // Oyuncunun CharacterController'ını kapat
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            Debug.Log("CharacterController KAPATILDI!");
        }

        // Oyuncuyu belirlenen noktaya ışınla
        player.position = targetPoint.position;
        Debug.Log($"Oyuncu yeni konuma taşındı: {player.position}");

        // Küçük bir bekleme süresi ekle (hareketin tamamlandığından emin olmak için)
        yield return new WaitForSeconds(0.2f);

        // CharacterController'ı tekrar aç
        if (controller != null)
        {
            controller.enabled = true;
            Debug.Log("CharacterController AÇILDI!");
        }

        // FADE IN (Ekranı tekrar aç)
        yield return new WaitForSeconds(transitionDelay / 2);

        isInside = !isInside; // İçerideyse dışarı, dışarıdaysa içeri geçir
        isTransitioning = false;
    }
}
