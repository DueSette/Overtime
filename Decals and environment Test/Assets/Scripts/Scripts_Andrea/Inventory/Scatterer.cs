using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //=====PURPOSE OF THE CLASS IS TO PROVIDE FUNCTIONALITIES FOR PROCEDURALLY SCATTERING NOTES THROUGHOUT LEVEL

public class Scatterer : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToScatter = new List<GameObject>();
    private List<ScatterReceiver> receivers = new List<ScatterReceiver>();

    //cycles through the list of scatterables and receivers, if item and slot have the same importance slot the item is assigned to the slot
    public void ScatterObjects()
    {
        foreach (GameObject obj in objectsToScatter)
        {
            try
            {
                InGameObjectBaseClass g = obj.GetComponent<InGameObjectBaseClass>();
                Scatter(g);
            }
            catch
            {
                throw new System.Exception("HELLO PAY ATTENTION: The object in the list is probably missing a component. Try adding NoteInGameObject or ItemInGameObject");
            }
        }
    }

    void Scatter(InGameObjectBaseClass scatterableItem) //add scatterReceiver
    {
        foreach (ScatterReceiver receiver in receivers)
        {
            if (receiver.importance == scatterableItem.importance) //if the importance is the same, place the object in the receiver's slot
            {
                PlaceItem(scatterableItem.gameObject, receiver.scatterSlot);
                receivers.Remove(receiver);
                break;
            }
        }
    }

    void PlaceItem(GameObject item, Transform slot)
    {
        GameObject spawnedObj = Instantiate(item, slot);
        spawnedObj.transform.localPosition = Vector3.zero;
    }

    #region SetUp Functions
    private void OnEnable()
    {
        ScatterReceiver.RaiseEvent += AddReceiver;
    }

    private void Start()
    {
        receivers.ShuffleList();
        objectsToScatter.ShuffleList();
        ScatterObjects();
    }

    private void OnDisable()
    {
        ScatterReceiver.RaiseEvent -= AddReceiver;
    }

    private void AddReceiver(ScatterReceiver s)
    {
        receivers.Add(s);
    }
    #endregion
}
