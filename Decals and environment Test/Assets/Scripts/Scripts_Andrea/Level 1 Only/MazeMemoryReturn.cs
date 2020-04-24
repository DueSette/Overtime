using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMemoryReturn : MonoBehaviour, IInteractable
{
    void IInteractable.InteractWith()
    {
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        PostProcessVolumeSummoner.instance.DarkTransition();
        yield return new WaitForSeconds(2f);

        // Warps The Player Back
        GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = false;

        GameStateManager.GetPlayer().transform.position = GameObject.FindGameObjectWithTag("MemoryReturnPoint").transform.position;
        GameStateManager.GetPlayer().GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().SetRotation(GameObject.FindGameObjectWithTag("MemoryReturnPoint").transform.rotation);

        GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = true;

        LevelManager.onLevelEvent("MemoryReturn");
    }
}
