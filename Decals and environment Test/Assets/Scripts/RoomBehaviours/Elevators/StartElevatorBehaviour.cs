using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevatorBehaviour : ElevatorBehaviour
{
    private void OnEnable()
    {
        LevelManager.onLevelEvent += LevelStart;
        LevelManager.onLevelEvent += OpenDoorsEvent;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= LevelStart;
        LevelManager.onLevelEvent -= OpenDoorsEvent;
    }

    private void OpenDoorsEvent(string eventCode)
    {
        if (eventCode == "PuzzleStart")
        {
            OpenDoors();
        }
    }
    private void LevelStart(string eventCode)
    {
        if (eventCode == "LevelStart")
        {
            StartCoroutine(LevelStartAnim());
        }
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
        yield return new WaitForSeconds(0.25f);


        Vector3 newPos = this.transform.position;
        newPos.y = 0;
        this.transform.position = newPos;

        // Starting Level
        LevelManager.onLevelEvent("PuzzleStart");
    }
}
