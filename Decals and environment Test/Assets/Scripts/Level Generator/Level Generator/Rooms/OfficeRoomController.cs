using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficeRoomController : MonoBehaviour
{
    [Header("Room Rect")]
    public int width;
    public int height;

    [Header("Room Details")]
    public string eventCode;
    public List<Connector> roomConnectors = new List<Connector>();
    public bool preSetConnectors = true;
    public GameObject wallPrefab;
    public GameObject doorPrefab;

    [Header("Generator Details")]
    public OfficeRoomController previousRoom;
    public bool placed = false;

    /*
    ====================================================================================================
    Controlling Room Elements
    ====================================================================================================
    */
    public void SetRoomSize(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;
    }


    /*
    ====================================================================================================
    Getting Room Info
    ====================================================================================================
    */
    public Rect GetRoomRect()
    {
        // Getting room size based on rotation
        Vector3 rotatedSize = new Vector3(width, 0, height);
        rotatedSize = this.transform.rotation * rotatedSize;

        // Room size cant be negative
        if (rotatedSize.x < 0)
        {
            rotatedSize.x *= -1;
        }
        if (rotatedSize.z < 0)
        {
            rotatedSize.z *= -1;
        }

        Rect roomRect = new Rect
        {
            x = this.transform.position.x - ((float)rotatedSize.x / 2),
            y = this.transform.position.z - ((float)rotatedSize.z / 2),
            width = rotatedSize.x,
            height = rotatedSize.z
        };

        return roomRect;
    }

    public List<Connector> GetRoomConnectors()
    {
        return roomConnectors;
    }
    public List<Connector> GetRoomConnectors(Directions connectorDirection)
    {
        int connectorDirectionInt = (int)connectorDirection;
        if (connectorDirectionInt >= 360)
        {
            connectorDirectionInt -= 360;
        }
        if (connectorDirectionInt < 0)
        {
            connectorDirectionInt += 360;
        }
        Directions direction = (Directions)connectorDirectionInt;

        List<Connector> sameTypeConnectors = new List<Connector>();
        for (int i = 0; i < roomConnectors.Count; i++)
        {
            if (roomConnectors[i].connectorDirection == direction)
            {
                sameTypeConnectors.Add(roomConnectors[i]);
            }
        }
        return (sameTypeConnectors);
    }

    public List<Connector> GetAvailableConnectors()
    {
        List<Connector> availableConnectors = new List<Connector>();

        for (int i = 0; i < roomConnectors.Count; i++)
        {
            if (!roomConnectors[i].connected)
            {
                availableConnectors.Add(roomConnectors[i]);
            }
        }

        return availableConnectors;
    }

    /*
    ====================================================================================================
    Rotating Room
    ====================================================================================================
    */
    public void SetRoomRotation(Directions newDirection)
    {
        int newRotation = (int)newDirection;

        if (newRotation >= 360)
        {
            newRotation -= 360;
        }
        if (newRotation < 0)
        {
            newRotation += 360;
        }

        // Sets parent object's rotation
        this.transform.eulerAngles = new Vector3
        {
            x = 0,
            y = Mathf.RoundToInt((int)newRotation),
            z = 0
        };

        for (int i = 0; i < roomConnectors.Count; i++)
        {
            roomConnectors[i].UpdateConnector();
        }
    }
    public void SetRoomRotation(int newRotation)
    {
        if (newRotation >= 360)
        {
            newRotation -= 360;
        }
        if (newRotation < 0)
        {
            newRotation += 360;
        }

        // Sets parent object's rotation
        this.transform.eulerAngles = new Vector3
        {
            x = 0,
            y = Mathf.RoundToInt(newRotation),
            z = 0
        };

        for (int i = 0; i < roomConnectors.Count; i++)
        {
            roomConnectors[i].UpdateConnector();
        }
    }

    /*
    ====================================================================================================
    Connecting Room
    ====================================================================================================
    */
    public void ConnectRoom(Connector thisRoomConnector, Connector otherRoomConnector)
    {
        // Rotating this room so that this connector is an opposite direction to the other connector's direction
        int connectorCurrentRotation = (int)thisRoomConnector.connectorDirection;
        int connectorTargetRotation = (int)Direction.directionOpposites[otherRoomConnector.connectorDirection];
        int roomCurrentRotation = (int)this.transform.eulerAngles.y;
        int roomNewRotation = (int)roomCurrentRotation + (connectorTargetRotation - connectorCurrentRotation);

        this.SetRoomRotation(roomNewRotation);

        // Alligning room to other connector
        Vector3 roomPosition = this.transform.position;
        Vector3 thisConnectionPos = thisRoomConnector.transform.position;
        Vector3 otherConnectionPos = otherRoomConnector.transform.position;
        
        this.transform.position = otherConnectionPos + (roomPosition - thisConnectionPos);
    }
    public void ConnectRoom(Connector thisRoomConnector, Vector3 positionToConnectTo)
    {
        Vector3 roomPosition = this.transform.position;
        Vector3 thisConnectionPos = thisRoomConnector.transform.position;
        Vector3 otherConnectionPos = positionToConnectTo;

        this.transform.position = otherConnectionPos + (roomPosition - thisConnectionPos);
    }


    /*
    ====================================================================================================
    Decorating
    ====================================================================================================
    */
    public void DecorateRoom()
    {
        if (!this.preSetConnectors)
        {
            foreach (Connector c in roomConnectors)
            {
                GameObject newPrefab;

                if (c.connected)
                {
                    newPrefab = Instantiate(doorPrefab, this.transform);
                }
                else
                {
                    newPrefab = Instantiate(wallPrefab, this.transform);
                }

                newPrefab.transform.position = c.transform.position;
                newPrefab.transform.rotation = c.transform.rotation;
            }
        }
    }



    /*
    ====================================================================================================
    Debugging
    ====================================================================================================
    */
    private void OnDrawGizmos()
    {
        // Drawing room bounds
        Gizmos.color = Color.cyan;

        Rect roomRect = this.GetRoomRect();
        Vector3 bottomLeft = new Vector3(roomRect.x, this.transform.position.y, roomRect.y);
        Vector3 bottomRight = new Vector3(roomRect.x + roomRect.width, this.transform.position.y, roomRect.y);
        Vector3 topLeft = new Vector3(roomRect.x, this.transform.position.y, roomRect.y + roomRect.height);
        Vector3 topRight = new Vector3(roomRect.x + roomRect.width, this.transform.position.y, roomRect.y + roomRect.height);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);

        // Drawing room tiles - Horizontal
        Gizmos.color = Color.blue;
        for (int i = 1; i < roomRect.height; i++)
        {
            Vector3 horizontalStart = bottomLeft + (Vector3.forward * i);
            Vector3 horizontalEnd = bottomRight + (Vector3.forward * i);

            Gizmos.DrawLine(horizontalStart, horizontalEnd);
        }

        // Drawing room tiles - vertical
        Gizmos.color = Color.blue;
        for (int i = 1; i < roomRect.width; i++)
        {
            Vector3 verticalStart = bottomLeft + (Vector3.right * i);
            Vector3 verticalEnd = topLeft + (Vector3.right * i);

            Gizmos.DrawLine(verticalStart, verticalEnd);
        }
    }
}