using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleScript : MonoBehaviour, IInteractable
{
    [SerializeField] Transform dial;
    [SerializeField] float rotationSpeed;
    [SerializeField] int[] numberSequence = { 9, 15, 27 };
    [SerializeField] AudioClip turningSound, correctSound, wrongSound;
    AudioSource aud;
    public int currentStep = 0;

    const int range = 6;
    int targetStep, targetIterator = 0; //which number to be checking for and which tick in the safe's sequence is the current correct number

    float safeDialTimer = 0.0f, checkThreshold = 1.65f;
    float proxyRotation = 3.0f; //internal value that keeps track of the dial's X rotation, since eulerAngles cannot be used here

    enum SafeState { PASSIVE, ACTIVE, SOLVED };
    SafeState safeState = SafeState.PASSIVE;

    enum TimerState { SUSPENDED = 0, INCREASING = 2};
    TimerState tState = TimerState.SUSPENDED;

    enum TurningState { LEFT, RIGHT, NONE };
    TurningState currentTurningDirection = TurningState.NONE;
    TurningState targetTurningDirection = TurningState.RIGHT;

    Quaternion startRot;
    private void Start()
    {
        aud = GetComponent<AudioSource>();
        startRot = dial.rotation;
        UpdateTargetSpot();
    }

    private void Update()
    {
        if (safeState != SafeState.ACTIVE) { return; }

        CheckInput();
        CheckDirection();

        CalculateCurrentStep();
        ManageTimer();

        if (safeDialTimer > checkThreshold)
        {
            tState = TimerState.SUSPENDED;

            targetIterator = CheckIfOnCorrectSpot() ? ++targetIterator : 0; //if player left the dial on correct value put next value on, else go back to value one
            
            UpdateTargetSpot();
        }
    }

    void CheckDirection()
    {
        if(currentTurningDirection == TurningState.NONE) { return; }

        if (currentTurningDirection != targetTurningDirection)
        {
            StartCoroutine(ResetSafe());
            aud.PlayOneShot(wrongSound);
            targetIterator = 0;
        }

        UpdateTargetSpot();
    }
 
    void ManageTimer() //updates timer value in accordance to current state
    {
        switch(tState)
        {
            case TimerState.INCREASING:
                safeDialTimer += Time.deltaTime;
                break;
            case TimerState.SUSPENDED:
                safeDialTimer = 0.0f;
                break;
        }
    }

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))//Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            tState = TimerState.SUSPENDED;
        else if (Input.GetMouseButtonUp(0))//if((Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            tState = TimerState.INCREASING;
            currentTurningDirection = TurningState.NONE;
        }

        if(!Input.GetMouseButton(0)) { return; }

        float horizontal = Input.GetAxis("Mouse X");
        if (horizontal > 0.08f || horizontal < -0.08f)
        {
            if (horizontal > 0) //(Input.GetKey(KeyCode.LeftArrow)) //for keyboard key controls
            {
                proxyRotation -= Time.deltaTime * rotationSpeed * horizontal;
                dial.Rotate(Vector3.left * rotationSpeed * horizontal * Time.deltaTime);
                currentTurningDirection = TurningState.LEFT;
            }
            else if (horizontal < 0) //Input.GetKey(KeyCode.RightArrow)) //for keyboard key controls
            {
                proxyRotation += Time.deltaTime * rotationSpeed * -horizontal;
                dial.Rotate(Vector3.left * rotationSpeed * horizontal * Time.deltaTime);
                currentTurningDirection = TurningState.RIGHT;
            }
        }
    }

    bool CheckIfOnCorrectSpot() //when enough time standing still on a tick has passed, it checks whether or not we are on the right tick
    {
        bool result = targetStep == currentStep;

        if(result)
            aud.PlayOneShot(correctSound);
        else
        {
            aud.PlayOneShot(wrongSound);
            StartCoroutine(ResetSafe());
        }

        return result;
    }

    void CalculateCurrentStep() //calculates what is the rotation expressed in degrees of the dial and maps it to the amount of available values
    {
        int oldStep = currentStep;

        if (proxyRotation < -0.1f) { proxyRotation = 357.0f; }
        else if (proxyRotation > 357.1f) { proxyRotation = 3.0f; }

        currentStep = (int)proxyRotation / range;
        if (oldStep != currentStep) { aud.PlayOneShot(turningSound); } //playus sound only if the player turned the dial enough
    }

    void UpdateTargetSpot() //sets new "correct" dial number and direction that the player has to leave the dial on
    {
        try
        {
            targetStep = numberSequence[targetIterator];
        }
        catch
        {
            safeState = SafeState.SOLVED;
            LeavePuzzle(Object);
        }
        targetTurningDirection = targetIterator % 2 == 0 ? TurningState.RIGHT : TurningState.LEFT;
    }

    IEnumerator ResetSafe() //resets safe to starting state
    {
        safeState = SafeState.PASSIVE;
        currentTurningDirection = TurningState.NONE;

        targetIterator = 0;
        proxyRotation = 3.0f;
        UpdateTargetSpot();

        float lapsed = 0.0f;
        float duration = 0.5f;

        Quaternion currRot = dial.rotation;
        while(lapsed < duration)
        {
            lapsed += Time.deltaTime;
            dial.rotation = Quaternion.Lerp(currRot, startRot, lapsed / duration);
            yield return null;
        }

        safeState = SafeState.ACTIVE;
    }

    void LeavePuzzle()
    {

    }

    void IInteractable.InteractWith()
    {
        if (safeState == SafeState.ACTIVE) { return; }

        safeState = SafeState.ACTIVE;
        //should also play animation
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the fusebox in focus

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += LeavePuzzle;
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= LeavePuzzle;
    }
}
