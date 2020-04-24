using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndElevatorBehaviour : ElevatorBehaviour
{
    [SerializeField] string nextLevel;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += OpenDoorsEvent;
        LevelManager.onLevelEvent += EndLevel;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= OpenDoorsEvent;
        LevelManager.onLevelEvent -= EndLevel;
    }

    void OpenDoorsEvent(string eventCode)
    {
        if (eventCode == "LevelSolved")
        {
            OpenDoors();
        }
    }
    void EndLevel(string eventCode)
    {
        if (eventCode == "EndLevel")
        {
            StartCoroutine(EndLevelAnim());
        }
    }
    IEnumerator EndLevelAnim()
    {
        // Closing Elevator Doors
        CloseDoors();
        yield return new WaitForSeconds(2.5f);

        // Moving Elevator Down
        while (this.transform.position.y > -3)
        {
            MoveElevator();
            yield return null;
        }

        // Getting Players Transform Relative To The End Elevator
        /*PlayerPositioning pp = PlayerPositioning.Instance;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.parent = this.transform;

        pp.playerPreviousPos = player.transform.localPosition;
        pp.playerPreviousRot = player.transform.localEulerAngles;*/

        // Loading Next Level
        GameObject lm = GameObject.FindGameObjectWithTag("GameController");
        if (lm != null)
        {
            LevelManager manager = lm.GetComponent<LevelManager>();
            nextLevel = manager.nextLevel;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }
}
