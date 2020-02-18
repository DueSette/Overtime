using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScatterImportance { CORE = 0, OPTIONAL = 2 }
public class ScatterReceiver : MonoBehaviour
{
    public delegate void SignalScatterReceiver(ScatterReceiver s);
    public static event SignalScatterReceiver RaiseEvent;

    public ScatterImportance importance = ScatterImportance.CORE;
    [Tooltip("The exact place where an item should be spawned")] public Transform scatterSlot;
    private bool usable = true;

    private void OnEnable()
    {
        if(usable)
            RaiseEvent(this);

        usable = false;
    }
}
