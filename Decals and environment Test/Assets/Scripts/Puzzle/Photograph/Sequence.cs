using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class Sequence : MonoBehaviour
{

    public GameObject player; // Tagged as Player
    public GameObject playerCamera; // Tagged as "MainCamera"
    public GameObject localPhotograph; // Assign this in Unity inspector. Is a child of the Photograph Sequence.
    public GameObject fadeScreen; // Assign Gameobject for this in the Unity Inspector. It is under canvas
    public Shader dissolveShader; // Assign Prefab for this in the Unity Inspector. Prefab not GameObject.
    public float dissolveLevel = 0f;
    public AudioSource dissolveSound; // Assign Prefab for this in the Unity Inspector. Should be a child of the FPS controller/Audiosources
    Renderer rend;

    public Material dissolveMaterial1; // Assign Prefab for this in the Unity Inspector. Prefab not GameObject.
    public Material dissolveMaterial2; // Assign Prefab for this in the Unity Inspector. Prefab not GameObject.
    public GameObject wall; // Tagged as "Pwall"
    public bool dissolveActive;
    public float noiseAmount;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   // Assigns variables by getting tag names
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        dissolveMaterial1.SetFloat("_DissolveAmount", dissolveLevel); 
        dissolveMaterial2.SetFloat("_DissolveAmount", dissolveLevel);
        noiseAmount = 0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        this.GetComponent<BoxCollider>().enabled = false; // Stops this trigger being activated a second time.
        StartCoroutine(ScenePlayer());
    }
    /*private void OnTriggerEnter()
    {
        this.GetComponent<BoxCollider>().enabled = false; // Stops this trigger being activated a second time.
        StartCoroutine(ScenePlayer());
    }*/

    private void Update()
    {
        dissolveMaterial1.SetFloat("_DissolveAmount", dissolveLevel);
        dissolveMaterial2.SetFloat("_DissolveAmount", dissolveLevel);

        dissolveLevel = dissolveLevel + noiseAmount;
    }

    IEnumerator ScenePlayer()
    {
        //fadeScreen.GetComponent<Animation>().Play("FadeInAndOut");
        player.transform.position = this.gameObject.transform.position;
        playerCamera.transform.rotation = this.gameObject.transform.rotation;
        player.GetComponent<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(2f);
        localPhotograph.GetComponent<Animation>().Play("PhotographSwitch");
        yield return new WaitForSeconds(4.5f);
        wall.SetActive(false);
        noiseAmount = 0.01f;
        //dissolveSound.Play();
        yield return new WaitForSeconds(1.9f);
        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
