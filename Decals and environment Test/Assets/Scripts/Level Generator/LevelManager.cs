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

    private void Start()
    {
        PickLayout();

        // Places the player in the start elevator of the selected level
        player.transform.position = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.position + PlayerPositioning.Instance.playerPreviousPos;
        player.transform.eulerAngles = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].transform.eulerAngles + PlayerPositioning.Instance.playerPreviousRot;

        player.GetComponent<CharacterController>().enabled = true;
        player.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
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
    private void NextLevel()
    {
        // Getting Players Transform Relative To The End Elevator
        PlayerPositioning pp = PlayerPositioning.Instance;
        pp.playerPreviousPos = player.transform.position - pickedLayout.GetRooms(RoomTypes.ELEVATOR_END)[0].transform.position;
        pp.playerPreviousRot = player.transform.eulerAngles - pickedLayout.GetRooms(RoomTypes.ELEVATOR_END)[0].transform.eulerAngles;

        // Loading Next Level
        SceneManager.LoadScene("LevelGeneration");
    }
}
