using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryRoomZeroScript : MonoBehaviour
{
    [Header("Mom Memory Event Objects")]
    public List<GameObject> furnitureToRemove = new List<GameObject>();
    [SerializeField] GameObject parent;
    private bool vanished = false;

    [Header("Memory Return Event Objects")]
    [SerializeField] Material dissolver;
    [SerializeField] Transform hallwaySpot;
    [SerializeField] private BadoomNotePickup badoomSpawner;
    [SerializeField] private ReflectionProbe cakeRoomReflectionProbe;
    [SerializeField] private List<GameObject> objectsToRemove;
    [SerializeField] private List<GameObject> objectsToAdd;

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
        if (!vanished)
        {
            foreach (GameObject g in furnitureToRemove)
            {
                SetForDissolutionWithChildren(g);
            }
            vanished = true;
        }
        else
        {
            SetForDissolutionWithChildren(parent);
            parent.GetComponent<Collider>().enabled = false;

            // Moving the player back to the office environment
            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = false;
            GameStateManager.GetPlayer().transform.position = hallwaySpot.position;
            GameStateManager.GetPlayer().GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().SetRotation(hallwaySpot.rotation);

            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = true;
            LevelManager.onLevelEvent("MemoryReturn");
            OpenableDoor.OnDoorUnlockEvent("MemoryReturn");

            // Changing the cake room to appear normal
            foreach (GameObject g in objectsToRemove)
            {
                g.SetActive(false);
            }
            foreach (GameObject g in objectsToAdd)
            {
                g.SetActive(true);
            }
            cakeRoomReflectionProbe.RenderProbe();

            // Removing Badooms
            badoomSpawner.EndBadoomSequence();
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

    IEnumerator WaitToRemove(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        obj.SetActive(false);
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
        StartCoroutine(WaitToRemove(obj));
    }
}
