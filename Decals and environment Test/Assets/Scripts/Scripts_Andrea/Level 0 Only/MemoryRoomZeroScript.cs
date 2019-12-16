using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryRoomZeroScript : MonoBehaviour
{
    public List<GameObject> fluffFurniture = new List<GameObject>();
    public List<GameObject> importantFurniture = new List<GameObject>();
    [SerializeField] GameObject[] parents;

    void OnEnable()
    {
        ParentScript.OnParentInteraction += VanishItem;
    }

    private void OnDisable()
    {
        ParentScript.OnParentInteraction -= VanishItem;
    }

    void VanishItem()
    {
        if(fluffFurniture.Count > 0)
        {
            int rand = Random.Range(0, fluffFurniture.Count);
            //use dissolve shader on object
            fluffFurniture.RemoveAt(rand);
        }
        else if (importantFurniture.Count > 0)
        {
            int rand = Random.Range(0, importantFurniture.Count);
            //use dissolve shader on object
            importantFurniture.RemoveAt(rand);
        }

        else
        {
            //same on parents
        }
    }
}
