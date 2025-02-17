//using System;
//using System.Collections.Generic;

//public static class EventBus<T> //Bu bir static sınıftır, yani nesne oluşturmadan (new EventBus()) doğrudan çağrılabilir. <T> → Generic (genel) türdür, farklı olay türleri (DoorEvent, GameEvent vb.) için kullanılabilir.
//{
//    private static readonly List<Action<T>> subscribers = new List<Action<T>>(); //Tüm olay dinleyicilerini (subscribers) saklayan bir liste oluşturduk. Bu liste, belirli bir olay (T) tetiklendiğinde çağrılacak tüm metotları içerir.

//    // Event'e abone ol (dinle)
//    public static void Subscribe(Action<T> callback) //Bu metot, olay dinlemek isteyenleri (subscribers listesine) ekler.
//    {
//        if (!subscribers.Contains(callback))
//        {
//            subscribers.Add(callback);
//        }
//    }

//    // Event'ten çık (dinleme)
//    public static void Unsubscribe(Action<T> callback)
//    {
//        if (subscribers.Contains(callback))
//        {
//            subscribers.Remove(callback);
//        }
//    }

//    // Event'i tetikle
//    public static void Raise(T eventData) //Bu metot, bir olay (eventData) meydana geldiğinde tüm dinleyicilere bildirir.
//    {
//        foreach (var subscriber in subscribers)
//        {
//            subscriber.Invoke(eventData);
//        }
//    }
//}
