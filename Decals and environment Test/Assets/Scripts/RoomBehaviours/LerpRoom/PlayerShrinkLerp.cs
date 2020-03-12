using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerShrinkLerp : MonoBehaviour
{
    private bool canLerp;
    private GameObject playerCharacter;
    private float furthestPoint = 0;

    [SerializeField] private float sizeMax;
    [SerializeField] private float sizeMin;

    [SerializeField] private CinemachinePathBase lerpRoomPath;

    /*
    ====================================================================================================
    Event Handling
    ====================================================================================================
    */
    private void OnEnable()
    {
        LevelManager.onLevelEvent += HandleEvent;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= HandleEvent;
    }
    void HandleEvent(string eventName)
    {
        if (eventName == "LerpRoom")
        {
            canLerp = true;
            playerCharacter = GameStateManager.GetPlayer();
        }

        if (eventName == "LevelSolved")
        {
            canLerp = false;
            playerCharacter.transform.localScale = Vector3.one;
        }
    }


    /*
    ====================================================================================================
    Lerping Player
    ====================================================================================================
    */
    // Update is called once per frame
    void Update()
    {
        if (canLerp)
        {
            LerpPlayer();
        }
    }

    private void LerpPlayer()
    {
        // Getting the closest point on the cinemachine path
        float newPoint = lerpRoomPath.FindClosestPoint(playerCharacter.transform.position, 0, -1, 10);

        // ensuring that the player doesn't grow in size
        if (newPoint > furthestPoint)
        {
            furthestPoint = newPoint;
        }

        // calculating new player height
        float maxPoint = lerpRoomPath.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits);
        float lerpPoint = furthestPoint / maxPoint;

        float newScale = Mathf.Lerp(sizeMax, sizeMin, lerpPoint);
        playerCharacter.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
