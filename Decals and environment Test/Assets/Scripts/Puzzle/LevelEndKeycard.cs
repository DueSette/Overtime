using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndKeycard : ItemInGameObjectScript
{
    protected override void InteractionEvent()
    {
        base.InteractionEvent();

        LevelManager.onLevelEvent("UnlockElevator");
    }
}
