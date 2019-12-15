using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public OfficeLayoutGenerator generator;
    private List<LayoutData> spawnedLayouts;
    private LayoutData pickedLayout;

    public GameObject player;

    public string nextLevel;

    // Level Events
    public delegate void LevelEvent();
    public static LevelEvent onEventLevelStart;
    /// <summary>
    /// Called When The Start Elevator Finishes it's Start Animation
    /// </summary>
    public static LevelEvent onEventPuzzleStart;
    public static LevelEvent onEventLevelSolved;
    /// <summary>
    /// Called When The End Elevator's Button Is Pressed
    /// </summary>
    public static LevelEvent onEventLevelEnd;

    private void Start()
    {
        // Chooses which variation of the level to use
        PickLayout();

        // Setting up start elevator
        Vector3 elevatorStartPos = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.position;
        elevatorStartPos.y += 3;
        pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.position = elevatorStartPos;

        // Places the player in the start elevator of the selected level
        player.transform.parent = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform;
        player.transform.localPosition = PlayerPositioning.Instance.playerPreviousPos;
        player.transform.localEulerAngles = PlayerPositioning.Instance.playerPreviousRot;
        player.transform.parent = null;

        player.GetComponent<CharacterController>().enabled = true;
        player.SetActive(true);

        // Starts the level
        onEventLevelStart();
    }


    /*
    ====================================================================================================
    Level Variation Handling
    ====================================================================================================
    */
    private void PickLayout()
    {
        spawnedLayouts = generator.spawnedLayouts;
        int r = Random.Range(0, spawnedLayouts.Count);
        pickedLayout = spawnedLayouts[r];

        for (int i = 0; i < spawnedLayouts.Count; i++)
        {
            if (i != r)
            {
                spawnedLayouts[i].gameObject.SetActive(false);
            }
        }
    }


    /*
    ====================================================================================================
    Level Loading
    ====================================================================================================
    */
    public void NextLevel()
    {
        // Getting Players Transform Relative To The End Elevator
        PlayerPositioning pp = PlayerPositioning.Instance;

        player.transform.parent = pickedLayout.GetRooms(RoomTypes.ELEVATOR_END)[0].transform;

        pp.playerPreviousPos = player.transform.localPosition;
        pp.playerPreviousRot = player.transform.localEulerAngles;

        // Loading Next Level
        SceneManager.LoadScene("LevelGeneration");
    }
}
