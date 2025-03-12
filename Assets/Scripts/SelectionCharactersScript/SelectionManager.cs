using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class SelectionManager
{
    public static int CurrentSelections { get; private set; }

    public static void AddSelection() => CurrentSelections++;
    public static void RemoveSelection() => CurrentSelections = Mathf.Max(0, CurrentSelections - 1);
    public static void Reset() => CurrentSelections = 0;
}
