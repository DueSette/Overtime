using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFactory : MonoBehaviour
{
    public List<RoomTypeFactory> roomTypeFactories;


    /*
    ====================================================================================================
    Factory Setup
    ====================================================================================================
    */
    public void SetupFactories()
    {
        foreach (RoomTypeFactory factory in roomTypeFactories)
        {
            factory.SetupFactory();
        }
    }


    /*
    ====================================================================================================
    Abstract Factory Design Pattern
    ====================================================================================================
    */
    public GameObject GetRoom(RoomTypes factoryType, int numberOfConnectors, string eventCode)
    {
        GameObject newRoom = null;

        // Getting factory to spawn from
        for (int i = 0;  i < roomTypeFactories.Count; i++)
        {
            if (roomTypeFactories[i].roomType == factoryType)
            {
                newRoom = roomTypeFactories[i].GetRoom(numberOfConnectors, eventCode);
            }
        }
        
        // Error Handling
        if (newRoom == null)
        {
            Debug.LogError("ERROR: No Suitable Room Found \nRoom Type: " + factoryType + "\nEvent Code: " + eventCode + "\nNumber of Required Connectors: " + numberOfConnectors);
        }

        return newRoom;
    }
}
