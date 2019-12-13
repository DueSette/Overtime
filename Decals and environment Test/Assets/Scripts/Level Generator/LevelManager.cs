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
    public delegate void LevelChange(Vector3 playerPos, Vector3 playerEulerRot);
    public static LevelChange onLevelChange;

    private void OnEnable()
    {
        onLevelChange += PositionPlayer;
    }

    private void Start()
    {
        // This level wasn't loaded by a previous level (For Debugging Purposes)
        if (pickedLayout == null)
        {
            PickLayout();

            // Places the player in the start elevator of the selected level
            player.transform.position = pickedLayout.layoutRooms[0].transform.position + (Vector3.up);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }


    /*
    ====================================================================================================
    Player Start
    ====================================================================================================
    */
    /// <summary></summary>
    /// <param name="playerPos">Player's position relative to the previous level's end elevator</param>
    /// <param name="playerEulerRot">Player's rotation in the previous level</param>
    private void PositionPlayer(Vector3 playerPos, Vector3 playerEulerRot)
    {
        GameObject startElevator = pickedLayout.GetRooms(RoomTypes.ELEVATOR_START)[0].gameObject;

        player.transform.position = startElevator.transform.position + playerPos;
        player.transform.eulerAngles = playerEulerRot;

        onLevelChange -= PositionPlayer;
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
    IEnumerator LoadNextLevel()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextLevel);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }
}
