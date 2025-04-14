using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DarknessEvents : MonoBehaviour
{
    public Action ondarknessBegin = () => { }; 
    public Action ondarknessEnd = () => { }; 


    public void DarknessBegin()
    {
        ondarknessBegin();
        ondarknessBegin = () => { };
    }

    public void DarknessEnd()
    {
        ondarknessEnd();
        ondarknessEnd = () => { };
    }
}
