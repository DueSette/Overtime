using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevatorBehaviour : ElevatorBehaviour
{
    private void OnEnable()
    {
        LevelManager.onEventLevelStart += LevelStart;

        LevelManager.onEventPuzzleStart += OpenDoors;
    }
    private void OnDisable()
    {
        LevelManager.onEventLevelStart -= LevelStart;

        LevelManager.onEventPuzzleStart -= OpenDoors;
    }

    private void LevelStart()
    {
        StartCoroutine(LevelStartAnim());
    }
    private IEnumerator LevelStartAnim()
    {
        // Moving Elevator Down
        while (this.transform.position.y > 0)
        {
            MoveElevator();
            yield return null;
        }

        // Setting position To Align With The Rest Of The Level
        StopElevator();

        Vector3 newPos = this.transform.position;
        newPos.y = 0;
        this.transform.position = newPos;

        // Starting Level
        LevelManager.onEventPuzzleStart();
    }
}
