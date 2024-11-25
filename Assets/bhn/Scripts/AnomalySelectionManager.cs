using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalySelectionManager : MonoBehaviour
{
    public static AnomalySelectionManager Instance { get; private set; }
    
    private int _anomalyCount;
    private int _normalCount;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Bu nesneyi sahneler arasında taşı
        }
        else
        {
            Destroy(gameObject); // Çift oluşumu engelle
        }
    }

    public void UpdateSelection(string tag, bool hasClicked)
    {
        if (tag == "Normal")
        {
            _normalCount += hasClicked ? 1 : -1;
        }
        
        else if (tag=="Abnormal")
        {
            _anomalyCount += hasClicked ? 1 : -1;
        }
        
        Debug.Log($"Normal: {_normalCount}, Abnormal: {_anomalyCount}");
    }

    public int GetNormalCount() => _normalCount;
    public int GetAbnormalCount() => _anomalyCount;
}
