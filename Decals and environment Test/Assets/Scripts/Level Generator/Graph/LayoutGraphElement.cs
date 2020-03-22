using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphElement", menuName = "Office Layout Graph")]
public class LayoutGraphElement : ScriptableObject
{
    public RoomTypes roomType;

    public List<LayoutGraphElement> connections;
    public List<string> connectionLockCodes; 
}

public enum RoomTypes
{
    BREAK_ROOM,
    CONFERENCE_ROOM,
    CORRIDOR,
    CUBICLE,
    ELEVATOR_START,
    ELEVATOR_END,
    KITCHEN,
    OFFICE,
    WAITING_AREA
}