using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryRoomZeroScript : MonoBehaviour
{
    public List<GameObject> fluffFurniture = new List<GameObject>();
    public List<GameObject> importantFurniture = new List<GameObject>();
    [SerializeField] GameObject parent;
    [SerializeField] Material dissolver;
    [SerializeField] Transform hallwaySpot;

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
        if (fluffFurniture.Count > 0)
        {
            int rand = Random.Range(0, fluffFurniture.Count);
            SetForDissolutionWithChildren(fluffFurniture[rand]);
            fluffFurniture.RemoveAt(rand);
        }

        else if (importantFurniture.Count > 0)
        {
            int rand = Random.Range(0, importantFurniture.Count);
            SetForDissolutionWithChildren(importantFurniture[rand]);
            importantFurniture.RemoveAt(rand);
        }

        else
        {
            SetForDissolutionWithChildren(parent);
            parent.GetComponent<Collider>().enabled = false;

            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = false;

            GameStateManager.GetPlayer().transform.position = hallwaySpot.position;
            GameStateManager.GetPlayer().GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().SetRotation(hallwaySpot.rotation);
            
            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = true;
            LevelManager.onLevelEvent("LevelSolved");
        }
    }

    //updates dissolve value of dissolve shader
    IEnumerator LerpDissolve(Material mat)
    {
        float lapsed = 0f;

        while (lapsed < 1)
        {
            lapsed += Time.deltaTime / 2;
            mat.SetFloat("_DissolveAmount", Mathf.Lerp(0, 1, lapsed));
            yield return null;
        }
    }

    void SetForDissolutionWithChildren(GameObject obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            obj.GetComponent<Renderer>().material = dissolver;
            StartCoroutine(LerpDissolve(obj.GetComponent<Renderer>().material));
        }
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            r.material = dissolver;
            StartCoroutine(LerpDissolve(r.material));
        }      
    }
}
