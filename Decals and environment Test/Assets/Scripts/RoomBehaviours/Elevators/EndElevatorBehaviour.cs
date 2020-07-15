using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndElevatorBehaviour : ElevatorBehaviour
{
    [SerializeField] private List<string> nextLevels;

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
        
        StartCoroutine(InventoriesManager.instance.FadeToBlack(5.5f));
        yield return new WaitForSeconds(2.5f);

        // Moving Elevator Down
        while (this.transform.position.y > -3)
        {
            MoveElevator();
            yield return null;
        }

        // Loading Next Level
        GameObject lm = GameObject.FindGameObjectWithTag("GameController");
        if (lm != null)
        {
            LevelManager manager = lm.GetComponent<LevelManager>();
            nextLevels = manager.nextLevels;
        }

        int i = Random.Range(0, nextLevels.Count);
        string nextLevel = nextLevels[i];

        SceneManager.LoadScene(nextLevel);
    }
}
