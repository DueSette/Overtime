using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInventory : MonoBehaviour
{
    [SerializeField] Transform inventoryCameraSpot;
    [SerializeField] Transform modelContainer;
    [SerializeField] Transform leftPanel;

    [SerializeField] TextMeshProUGUI descriptionBox;
    [SerializeField] GameObject entryNamePrefab;

    List<GameObject> itemNamesList = new List<GameObject>(); //list of all the UI items (their name, basically), we need this to have access to the UI entries in the inventory
    public List<Item> itemList = new List<Item>(); //a list of Type <Item> (a struct that contains info like model, name, description)

    [SerializeField] float rotationSpeed; //to rotate the items in view
    int currentFocus = 0;
    GameObject itemInView = null;

    [System.Serializable]
    public class Item
    {
        public GameObject model;
        public string name;
        public string description;
    }

    private void Start()
    {
        //populate itemList - possibly load from save
        int x = 0;

        foreach (Item i in itemList)
        {
            i.model = Instantiate(i.model, Vector3.zero, Quaternion.identity, modelContainer); //instantiate all owned items and set container as parent
            i.model.transform.localPosition = Vector3.zero;

            //i.model.layer = LayerMask.NameToLayer("ViewableObjects"); //need this layer so that the dedicated camera will only render on that layer    
            SetObjectWithChildrenOnLayer(i.model);

            itemNamesList.Add(Instantiate(entryNamePrefab, leftPanel));
            itemNamesList[x].GetComponent<TextMeshProUGUI>().text = i.name;
            itemNamesList[x].GetComponent<ItemUIObjectScript>().containedItem = i;
            x++;
        }
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (itemInView != null && Input.GetMouseButton(1))
            RotateItem();
    }

    #region Reoccurring Methods
    public void UnlockNewItem(Item newItem) //adds a new item to the inventory
    {
        currentFocus = itemList.Count;

        for (int i = 0; i < itemList.Count; i++)
            if (itemList[i] == newItem) //if the new item is actually an Item that is still on the map but is clicked again
                currentFocus = i;

        foreach (Item i in itemList)  //if we already are in possess of this item don't add it, but still open the inventory       
            if (i == newItem)
                return;

        itemList.Add(newItem);

        newItem.model = Instantiate(newItem.model, Vector3.zero, Quaternion.identity, modelContainer);
        newItem.model.transform.localPosition = Vector3.zero;

        //newItem.model.layer = LayerMask.NameToLayer("ViewableObjects");
        SetObjectWithChildrenOnLayer(newItem.model);

        itemNamesList.Add(Instantiate(entryNamePrefab, leftPanel));
        itemNamesList[itemNamesList.Count - 1].GetComponent<TextMeshProUGUI>().text = newItem.name;
    }

    public void UpdateUI() //updates render texture, description, entry names to the currently selected item
    {
        if (itemNamesList.Count < 1)
            return;

        currentFocus = Mathf.Clamp(currentFocus, 0, itemList.Count - 1);

        PutItemInView(itemList[currentFocus].model);
        descriptionBox.text = itemList[currentFocus].description;

        itemNamesList[currentFocus].GetComponent<TextMeshProUGUI>().color = Color.red;
        itemNamesList[currentFocus].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }
    
    void SetObjectWithChildrenOnLayer(GameObject model)
    {
        model.layer = LayerMask.NameToLayer("ViewableObjects");

        int x = model.transform.childCount;

        for (int i = 0; i < x; i++)
        {
            GameObject child = model.transform.GetChild(i).gameObject;
            SetObjectWithChildrenOnLayer(child);
        }
    } //Recursively assigns correct layer to object and all it's children

    public void ScrollItems(bool up) //navigates through the list
    {
        CleanPreviousUI();
        currentFocus += up ? -1 : 1;
        currentFocus = Mathf.Clamp(currentFocus, 0, itemList.Count - 1);
        UpdateUI();
    }

    public void ScrollItemsFromUI(Item i) //this is called via item inventory UI when clicking on an entry
    {
        int x = 0;
        foreach (Item it in itemList) //we search through the owned items, when one matches we set it as the current focus and update UI accodingly
        {
            if (i == it)
            {
                CleanPreviousUI();
                currentFocus = x;
                UpdateUI();
                break;
            }
            x++;
        }


    }

    public void CleanPreviousUI()  //just reverts some changes caused by highlighting an item in the list
    {
        if (itemNamesList.Count < 1)
            return;

        itemNamesList[currentFocus].GetComponent<TextMeshProUGUI>().color = Color.white;
        itemNamesList[currentFocus].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

        //maybe make it lerp away
        if (itemInView != null)
            itemInView.SetActive(false);
    }
    #endregion

    #region 3D Item Manipulation - logics that handles the actual 3D models
    void PutItemInView(GameObject model) //puts the 3d model in front of the dedicated camera
    {
        itemInView = model;
        model.SetActive(true);
        model.transform.position = inventoryCameraSpot.position;
    }

    void RotateItem() //for some reason non-y axes need to rotate this way
    {
        float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
        float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

        itemInView.transform.Rotate(Vector3.up, -xAxis, Space.Self);
        itemInView.transform.RotateAround(itemInView.GetComponent<Collider>().bounds.center, Vector3.right, yAxis);
    }
    #endregion
}
