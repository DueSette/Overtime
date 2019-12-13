using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    private static PlayerPositioning _instance;
    public static PlayerPositioning Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject g = new GameObject("PlayerPositioningData");
                _instance = g.AddComponent<PlayerPositioning>();
            }

            return _instance;
        }
    }

    public Vector3 playerPreviousPos = Vector3.up;
    public Vector3 playerPreviousRot = Vector3.zero;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }
}
