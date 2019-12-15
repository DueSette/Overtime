using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndElevatorBehaviour : ElevatorBehaviour
{
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

        // Moving To The Next Level
        LevelManager lm = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        lm.NextLevel();
    }
}
