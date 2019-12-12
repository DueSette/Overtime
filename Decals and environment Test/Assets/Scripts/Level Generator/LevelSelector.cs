using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public OfficeLayoutGenerator generator;
    private List<LayoutData> spawnedLayouts;

    public GameObject player;

    private LayoutData pickedLayout;

    // Start is called before the first frame update
    void Start()
    {
        spawnedLayouts = generator.spawnedLayouts;

        int r = Random.Range(0, spawnedLayouts.Count);
        pickedLayout = spawnedLayouts[r];

        for (int i = 0; i < spawnedLayouts.Count; i++)
        {
            if (i != r)
            {
                spawnedLayouts[i].gameObject.SetActive(false);
            }
        }

        player.transform.position = pickedLayout.layoutRooms[4].transform.position + (Vector3.up);
        player.GetComponent<CharacterController>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
