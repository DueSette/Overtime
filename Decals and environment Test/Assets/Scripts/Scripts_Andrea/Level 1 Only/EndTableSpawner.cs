using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTableSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToSpawn;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += SpawnObjects;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= SpawnObjects;
    }

    void SpawnObjects(string eventCode)
    {
        if (eventCode == "MemoryReturn")
        {
            foreach (GameObject g in objectsToSpawn)
            {
                g.SetActive(true);
            }
        }
    }
}
