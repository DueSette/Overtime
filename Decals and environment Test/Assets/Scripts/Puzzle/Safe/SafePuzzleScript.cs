using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleScript : MonoBehaviour, IInteractable
{
    [SerializeField] Transform dial;
    [SerializeField] float rotationSpeed;
    [SerializeField] int[] numberSequence = { 9, 15, 27 };

    public int currentStep = 0;

    const int range = 6;
    int targetStep, targetIterator = 0; //which number to be checking for and which tick in the safe's sequence is the current correct number

    float safeDialTimer = 0.0f, checkThreshold = 1.65f;
    float proxyRotation = 3.0f; //internal value that keeps track of the dial's X rotation, since eulerAngles cannot be used here

    bool inUse = false;

    enum TimerState { SUSPENDED = 0, INCREASING = 2};
    TimerState tState = TimerState.SUSPENDED;

    enum TurningState { LEFT, RIGHT, NONE };
    TurningState currentTurningDirection = TurningState.NONE;
    TurningState targetTurningDirection = TurningState.RIGHT;

    private void Start()
    {
        UpdateTargetSpot();
    }

    private void Update()
    {
        if (!inUse) { return; }

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

        if (currentTurningDirection != targetTurningDirection) { targetIterator = 0; }

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
        return targetStep == currentStep;
    }

    void CalculateCurrentStep() //calculates what is the rotation expressed in degrees of the dial and maps it to the amount of available values
    {
        if (proxyRotation < -0.1f) { proxyRotation = 357.0f; }
        else if (proxyRotation > 357.1f) { proxyRotation = 3.0f; }

        currentStep = (int)proxyRotation / range;
    }

    void UpdateTargetSpot() //sets new "correct" dial number and direction that the player has to leave the dial on
    {
        targetStep = numberSequence[targetIterator];
        targetTurningDirection = targetIterator % 2 == 0 ? TurningState.RIGHT : TurningState.LEFT;
    }

    void IInteractable.InteractWith()
    {
        if (inUse) { return; }

        inUse = true;
        //should also play animation
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the fusebox in focus

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
