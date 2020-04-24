using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public OfficeLayoutGenerator generator;
    private List<LayoutData> spawnedLayouts;
    private LayoutData pickedLayout;

    public string nextLevel;

    // Level Events
    public delegate void LevelEvent(string eventCode);
    public static LevelEvent onLevelEvent;

    private void Start()
    {
        // Chooses which variation of the level to use
        PickLayout();

        // Saving that the player has reached level 1
        PlayerPrefs.SetInt("CanContinue", 1);

        // Setting up start elevator
        Vector3 elevatorStartPos = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.position;
        elevatorStartPos.y += 3;
        pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.position = elevatorStartPos;

        // Places the player in the start elevator of the selected level
        GameObject player = GameStateManager.GetPlayer();
        player.GetComponent<CharacterController>().enabled = false;
        player.SetActive(false);

        player.transform.parent = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform;
        player.transform.localPosition = PlayerPositioning.Instance.playerPreviousPos;
        player.transform.localEulerAngles = PlayerPositioning.Instance.playerPreviousRot;
        player.transform.parent = null;


        player.GetComponent<CharacterController>().enabled = true;
        player.SetActive(true);

        // Starts the level
        onLevelEvent("LevelStart");
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
}
