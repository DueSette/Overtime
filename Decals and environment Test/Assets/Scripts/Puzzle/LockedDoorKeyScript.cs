using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorKeyScript : ItemInGameObjectScript
{
    [SerializeField]
    private OpenableDoor unlockingDoor;

    protected override void InteractionEvent()
    {
        unlockingDoor.canBeOpened = true;
    }
}
