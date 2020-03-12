using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeScript : MonoBehaviour, ITextPrompt, IInteractable
{
    [SerializeField] Transform memoryRoomSpawnPoint;
    void IInteractable.InteractWith()
    {
        PostProcessVolumeSummoner.instance.DarkTransition();
        StartCoroutine(BringToMemory());
    }

    string ITextPrompt.PromptText()
    {
        return "This does not look right";
    }

    private IEnumerator BringToMemory()
    {
        yield return new WaitForSeconds(1.5f);
        GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = false;

        GameStateManager.GetPlayer().transform.position = memoryRoomSpawnPoint.position;
        GameStateManager.GetPlayer().GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().SetRotation(memoryRoomSpawnPoint.rotation);

        GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = true;

        LevelManager.onLevelEvent("LerpRoom");
    }
}
