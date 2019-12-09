using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFactory : RoomTypeFactory
{
    public bool premadeCorridor = false;

    public GameObject expandingCorridorPrefab;

    public override GameObject GetRoom(int requiredConnections, string eventCode)
    {
        if (premadeCorridor)
        {
            return base.GetRoom(requiredConnections, eventCode);
        }
        else
        {
            return expandingCorridorPrefab;
        }
    }
}
