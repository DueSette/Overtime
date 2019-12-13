using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutData : MonoBehaviour
{
    public List<OfficeRoomController> layoutRooms = new List<OfficeRoomController>();

    public List<OfficeRoomController> GetRooms(RoomTypes roomType)
    {
        List<OfficeRoomController> rooms = new List<OfficeRoomController>();

        for (int i = 0; i < layoutRooms.Count; i++)
        {
            if (layoutRooms[i].roomType == roomType)
            {
                rooms.Add(layoutRooms[i]);
            }
        }

        return rooms;
    }
}