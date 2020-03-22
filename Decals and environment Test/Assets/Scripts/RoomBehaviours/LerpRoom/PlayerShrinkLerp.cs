using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerShrinkLerp : MonoBehaviour
{
    private bool canLerp;
    private CharacterController playerCharacterController;
    private GameObject floorController;
    private float furthestPoint = 0;

    [SerializeField] private float heightMax;
    [SerializeField] private float heightMin;
    
    [SerializeField] private float floorOffsetStart;
    [SerializeField] private float floorOffsetEnd;

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
            playerCharacterController = GameStateManager.GetPlayer().GetComponent<CharacterController>();
            floorController = GameStateManager.GetPlayer().GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_FloorDetector.gameObject;
        }

        if (eventName == "LevelSolved")
        {
            canLerp = false;
            playerCharacterController.height = heightMax;
            floorController.transform.localPosition = new Vector3(0, floorOffsetStart, 0);
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
        float newPoint = lerpRoomPath.FindClosestPoint(playerCharacterController.transform.position, 0, -1, 10);

        // ensuring that the player doesn't grow in size
        if (newPoint > furthestPoint)
        {
            furthestPoint = newPoint;
        }

        float maxPoint = lerpRoomPath.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits);
        float lerpPoint = furthestPoint / maxPoint;

        // calculating new player height
        float newHeight = Mathf.Lerp(heightMax, heightMin, lerpPoint);
        playerCharacterController.height = newHeight;

        // Changing Floor Controller Offset to work with new player height
        float newOffset = Mathf.Lerp(floorOffsetStart, floorOffsetEnd, lerpPoint);
        floorController.transform.localPosition = new Vector3(0, newOffset, 0);
    }
}
