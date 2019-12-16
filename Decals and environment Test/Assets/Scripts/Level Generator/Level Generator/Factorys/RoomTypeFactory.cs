using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypeFactory : MonoBehaviour
{
    public RoomTypes roomType;

    public List<GameObject> prefabRooms;
    public List<GameObject> currentRooms;

    /*
    ====================================================================================================
    Factory Setup
    ====================================================================================================
    */
    public void SetupFactory()
    {
        currentRooms = new List<GameObject>();
        for (int i = 0; i < prefabRooms.Count; i++)
        {
            currentRooms.Add(prefabRooms[i]);
        }
    }


    /*
    ====================================================================================================
    Factory Design Pattern
    ====================================================================================================
    */
    public virtual GameObject GetRoom(int requiredConnections, string eventCode)
    {
        GameObject roomToReturn = null;

        // Gets all rooms with a suitable number of room entrances
        List<GameObject> possibleRooms = new List<GameObject>();
        for (int i = 0; i < currentRooms.Count; i++)
        {
            OfficeRoomController orc = currentRooms[i].GetComponent<OfficeRoomController>();
            // Checks whether the room has hard designed connections or not 
            if (!orc.preSetConnectors)
            {
                // Room can have any number of connectors as these will be decorated later
                possibleRooms.Add(currentRooms[i]);
            }
            else
            {
                // Number of room connectors must match the required amount
                if (orc.roomConnectors.Count == requiredConnections)
                {
                    possibleRooms.Add(currentRooms[i]);
                }
            }
        }

        // First - searches for event rooms
        // Event rooms have hard defined entrances and exits
        List<GameObject> eventRooms = new List<GameObject>();
        for (int i = 0; i < possibleRooms.Count; i++)
        {
            OfficeRoomController orc = possibleRooms[i].GetComponent<OfficeRoomController>();
            if (orc.eventCode == eventCode)
            {
                eventRooms.Add(possibleRooms[i]);
            }
        }
        if (eventRooms.Count > 0)
        {
            // Returning an event room
            roomToReturn = eventRooms[Random.Range(0, eventRooms.Count)];
            return roomToReturn;
        }

        // Second - No event rooms to spawn or event rooms have already been spawned
        if (eventRooms.Count == 0)
        {
            List<GameObject> plainRooms = new List<GameObject>();
            for (int i = 0; i < possibleRooms.Count; i++)
            {
                if (prefabRooms[i].GetComponent<OfficeRoomController>().eventCode == "")
                {
                    plainRooms.Add(prefabRooms[i]);
                }
            }
            if (plainRooms.Count > 0)
            {
                // Returning a plain room
                roomToReturn = plainRooms[Random.Range(0, plainRooms.Count)];
                return roomToReturn;
            }
        }

        // No suitable room found, returns null
        return roomToReturn;
    }
}
