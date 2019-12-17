using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndElevatorBehaviour : ElevatorBehaviour
{
    [SerializeField] string nextLevel;

    private void OnEnable()
    {
        LevelManager.onEventLevelSolved += OpenDoors;

        LevelManager.onEventLevelEnd += EndLevel;
    }
    private void OnDisable()
    {
        LevelManager.onEventLevelSolved -= OpenDoors;

        LevelManager.onEventLevelEnd -= EndLevel;
    }

    void EndLevel()
    {
        StartCoroutine(EndLevelAnim());
    }
    IEnumerator EndLevelAnim()
    {
        // Closing Elevator Doors
        CloseDoors();
        yield return new WaitForSeconds(1);

        // Moving Elevator Down
        while (this.transform.position.y > -3)
        {
            MoveElevator();
            yield return null;
        }

        // Getting Players Transform Relative To The End Elevator
        PlayerPositioning pp = PlayerPositioning.Instance;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.parent = this.transform;

        pp.playerPreviousPos = player.transform.localPosition;
        pp.playerPreviousRot = player.transform.localEulerAngles;

        // Loading Next Level
        LevelManager lm = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        if (lm != null)
        {
            nextLevel = lm.nextLevel;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }
}
