using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndKeycard : ItemInGameObjectScript
{
    protected override void InteractionEvent()
    {
        LevelManager.onEventLevelSolved();
    }
}
