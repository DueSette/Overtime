using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorKeyScript : ItemInGameObjectScript
{
    [SerializeField]
    private string keyUnlockEventCode;

    protected override void OnInteraction()
    {
        base.OnInteraction();
        Debug.Log("Picked Up Door Key: " + keyUnlockEventCode);
        OpenableDoor.OnDoorUnlockEvent(keyUnlockEventCode);
    }
}
