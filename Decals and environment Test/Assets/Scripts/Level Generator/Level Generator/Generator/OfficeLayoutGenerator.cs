using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfficeLayoutGenerator : MonoBehaviour
{
    [Header("Generator Input - Floorplan Graph Representation")]
    public LayoutGraphElement graphStart;

    [Header("Generator Input - Possible Rooms To Spawn")]
    public AbstractFactory abstractFactory;
    
    public string eventCode;

    private List<OfficeRoomController> spawnedRooms = new List<OfficeRoomController>();
    private Dictionary<LayoutGraphElement, OfficeRoomController> spawnedRoomDict = new Dictionary<LayoutGraphElement, OfficeRoomController>();
    
    private bool generationFailed = false;

    public int numberOfAttempts = 100;
    private int currentAttempt = 0;
    public int numberOfGenerations = 10;
    private int currentGeneration = 0;

    public List<LayoutData> spawnedLayouts;

    /*
    ====================================================================================================
    Office Generation
    ====================================================================================================
    */
    public void StartNewGeneration(string eventCode)
    {
        this.transform.position = new Vector3(25, 0, 0);

        spawnedRooms = new List<OfficeRoomController>();
        spawnedRoomDict = new Dictionary<LayoutGraphElement, OfficeRoomController>();

        generationFailed = false;

        currentAttempt = 0;
        currentGeneration = 0;

        
        spawnedLayouts = new List<LayoutData>();

        StartCoroutine(Generate(eventCode));
    }
    public IEnumerator Generate(string eventCode)
    {
        // STEP 1 - Setup
        // Clearing Previous Generation
        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            if (spawnedRooms[i] != null)
            {
                GameObject g = spawnedRooms[i].gameObject;

                spawnedRooms[i] = null;

                DestroyImmediate(g);
            }
        }
        // Starting Generation
        abstractFactory.SetupFactories();

        // Setup spawned room tracking
        spawnedRooms = new List<OfficeRoomController>();
        spawnedRoomDict = new Dictionary<LayoutGraphElement, OfficeRoomController>();

        generationFailed = false;

        // STEP 2 - Spawning Relevant Rooms Based On Level Event Code
        SpawnRoom(graphStart);

        PlaceElevatorsAndCorridors();
        PlaceRooms(graphStart);

        if (generationFailed)
        {
            // Try Again
            if (currentAttempt < numberOfAttempts)
            {
                yield return new WaitForSeconds(0);
                StartCoroutine(Generate(eventCode));
                currentAttempt++;
            }
            // Failure State, No Possible Solution Might Exist
            else
            {
                Debug.LogError("ERROR: Generator Was Not Able To Find Solution");

                // Removing all spawned rooms
                for (int i = 0; i < spawnedRooms.Count; i++)
                {
                    if (spawnedRooms[i] != null)
                    {
                        GameObject g = spawnedRooms[i].gameObject;

                        spawnedRooms[i] = null;

                        DestroyImmediate(g);
                    }
                }
            }
        }
        else
        {
            // Finishing Touches
            DecorateRooms();

            // Clean Up Current Generation
            currentGeneration++;
            GameObject newParent = new GameObject(SceneManager.GetActiveScene().name + " (Generation " + currentGeneration + ") (" + System.DateTime.Now + ")");
            newParent.transform.position = this.transform.position;

            foreach (OfficeRoomController ofc in spawnedRooms)
            {
                ofc.transform.parent = newParent.transform;
            }

            LayoutData ld = newParent.AddComponent<LayoutData>();
            ld.layoutRooms = spawnedRooms;

            spawnedLayouts.Add(ld);

            if (currentGeneration < numberOfGenerations)
            {
                this.transform.position = new Vector3
                {
                    x = this.transform.position.x + 25,
                    y = this.transform.position.y,
                    z = this.transform.position.z
                };

                spawnedRooms = new List<OfficeRoomController>();
                spawnedRoomDict = new Dictionary<LayoutGraphElement, OfficeRoomController>();

                generationFailed = false;

                currentAttempt = 0;

                StartCoroutine(Generate(eventCode));
            }
            else
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
    }


    /*
    ====================================================================================================
    Step 1 - Room Spawning
    ====================================================================================================
    */
    private void SpawnRoom(LayoutGraphElement currentRoom)
    {
        int numberOfConnectors = currentRoom.connections.Count;
        if (currentRoom.roomType != RoomTypes.ELEVATOR_START)
        {
            numberOfConnectors++;
        }

        GameObject newRoom = abstractFactory.GetRoom(currentRoom.roomType, numberOfConnectors, eventCode);

        OfficeRoomController newroomController = Instantiate(newRoom).GetComponent<OfficeRoomController>();
        spawnedRooms.Add(newroomController);
        spawnedRoomDict.Add(currentRoom, newroomController);

        spawnedRoomDict.TryGetValue(currentRoom, out newroomController.previousRoom);

        for (int i = 0; i < currentRoom.connections.Count; i++)
        {
            // Continue down graph tree
            SpawnRoom(currentRoom.connections[i]);
        }
    }


    /*
    ====================================================================================================
    Step 2 - Placing Spawned Rooms To Avoid Overlap
    ====================================================================================================
    */
    private void PlaceElevatorsAndCorridors()
    {
        // Start Elevator
        OfficeRoomController startElevator = spawnedRoomDict[graphStart];
        startElevator.ConnectRoom(startElevator.GetRoomConnectors()[0], this.transform.position);
        startElevator.placed = true;

        // Corridor
        OfficeRoomController corridor = spawnedRoomDict[graphStart.connections[0]];
        corridor.ConnectRoom(corridor.GetRoomConnectors(Directions.DOWN)[0], startElevator.GetRoomConnectors()[0]);

        corridor.previousRoom = startElevator;
        corridor.placed = true;

        // End Elevator
        OfficeRoomController endElevator = spawnedRoomDict[graphStart.connections[0].connections[0]];
        endElevator.ConnectRoom(endElevator.GetRoomConnectors()[0], corridor.GetRoomConnectors(Directions.UP)[0]);

        endElevator.previousRoom = corridor;
        endElevator.placed = true;
    }
    private void PlaceRooms(LayoutGraphElement currentElement)
    {
        for (int i = 0; i < currentElement.connections.Count; i++)
        {
            // Getting relevant spawned room
            OfficeRoomController nextRoom = spawnedRoomDict[currentElement.connections[i]];

            if (!nextRoom.placed)
            {
                // Rooms To Connect to
                OfficeRoomController previousRoom = spawnedRoomDict[currentElement];

                // All the possible connection points on the rooms
                List<Connector> previousRoomConnectionPoints = previousRoom.GetAvailableConnectors();
                List<Connector> nextRoomConnectionPoints = nextRoom.GetAvailableConnectors();

                // List of all possible combinations
                List<System.Tuple<Connector, Connector>> allConnectionCombinations = new List<System.Tuple<Connector, Connector>>();
                foreach (Connector nextConnector in nextRoomConnectionPoints)
                {
                    foreach (Connector prevConnector in previousRoomConnectionPoints)
                    {
                        allConnectionCombinations.Add(new System.Tuple<Connector, Connector>(nextConnector, prevConnector));
                    }
                }

                // Test all possible combinations
                List<System.Tuple<Connector, Connector>> possibleConnections = new List<System.Tuple<Connector, Connector>>();
                foreach (System.Tuple<Connector, Connector> c in allConnectionCombinations)
                {
                    //Picking Connectors To Test
                    Connector connectionNext = c.Item1;
                    Connector connectionPrevious = c.Item2;

                    // Checking if connectors are opposite of each other
                    if (connectionNext.connectorDirection != Direction.directionOpposites[connectionPrevious.connectorDirection])
                    {
                        // Rotate next room to line up with previous room's connector
                        int connectorCurrentRotation = (int)connectionNext.connectorDirection;
                        int connectorTargetRotation = (int)Direction.directionOpposites[connectionPrevious.connectorDirection];
                        int roomCurrentRotation = (int)nextRoom.transform.eulerAngles.y;
                        int roomNewRotation = (int)roomCurrentRotation + (connectorTargetRotation - connectorCurrentRotation);

                        nextRoom.SetRoomRotation(roomNewRotation);
                    }

                    // Connecting Rooms
                    nextRoom.ConnectRoom(connectionNext, connectionPrevious);

                    // Checking Overlap
                    if (!OverlappingWithPlacedRooms(nextRoom))
                    {
                        possibleConnections.Add(c);
                    }
                }

                //Generation Failure
                if (possibleConnections.Count == 0)
                {
                    generationFailed = true;
                }
                else
                {
                    //Picking Connectors To Test - Random Selection 
                    System.Tuple<Connector, Connector> c = possibleConnections[Random.Range(0, possibleConnections.Count)];
                    Connector connectionNext = c.Item1;
                    Connector connectionPrevious = c.Item2;

                    // Connecting Rooms
                    nextRoom.ConnectRoom(connectionNext, connectionPrevious);

                    connectionNext.connected = true;
                    connectionPrevious.connected = true;
                    nextRoom.placed = true;
                }
            }

            if (!generationFailed)
            {
                // Continue down graph tree
                PlaceRooms(currentElement.connections[i]);
            }
        }
    }


    /*
    ====================================================================================================
    Step 3 - Decorating Placed Rooms
    ====================================================================================================
    */
    private void DecorateRooms()
    {
        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            OfficeRoomController ofc = spawnedRooms[i];
            ofc.DecorateRoom();
        }
    }


    /*
    ====================================================================================================
    Intersection Checking
    ====================================================================================================
    */
    public bool OverlappingWithPlacedRooms(OfficeRoomController roomToCheck)
    {
        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            if (spawnedRooms[i].placed)
            {
                // Check if any of the new room corners are inside the spawned room rect
                Rect spawnedRoom = spawnedRooms[i].GetRoomRect();
                // Check if any of the spawned room corners are inside the new room rect
                Rect newRoom = roomToCheck.GetRoomRect();

                if (spawnedRoom.Overlaps(newRoom, true) || newRoom.Overlaps(spawnedRoom, true))
                {
                    //Debug.Log(roomToCheck + " Overlaps: " + spawnedRooms[i]);
                    //Debug.Log(newRoom + " Overlaps: " + spawnedRoom);
                    return true;
                }
            }
        }

        return false;
    }
    private bool OverlappingWithOtherRoom(OfficeRoomController roomToCheck, OfficeRoomController otherRoom)
    {
        Rect room1 = roomToCheck.GetRoomRect();
        Rect room2 = otherRoom.GetRoomRect();

        room1.x += 0.1f;
        room1.y += 0.1f;
        room1.width -= 0.2f;
        room1.height -= 0.2f;

        if (room1.Overlaps(room2, true) || room2.Overlaps(room1, true))
        {
            Debug.Log(roomToCheck + " Overlaps: " + otherRoom);
            Debug.Log(room1 + " Overlaps: " + room2);
            return true;
        }
        else return false;
    }
}