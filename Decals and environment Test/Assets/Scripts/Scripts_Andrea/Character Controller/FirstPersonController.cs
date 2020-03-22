using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Walking and running")]
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private bool m_IsCrouching = false;
        [SerializeField] private float accelerationRate;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField, Range(0.2f, 0.95f)] private float m_CrouchSpeedModifier;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;

        [Space, Header("Camera settings")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float m_CameraZoomFOV = 30f;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;

        [Header("Sounds")]
        [SerializeField] private AudioClip[] m_WoodFootstepSounds;    // an array of footstep sounds that will be randomly selected from, one for each kind
        [SerializeField] private AudioClip[] m_CarpetFootstepSounds;
        [SerializeField] private AudioClip[] m_ConcreteFootstepSounds;
        [Space]
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] Transform m_FloorDetector;
        [SerializeField] Transform m_CeilingDetector;

        private bool m_Jump;
        private float m_YRotation;
        float acceleration = 0.0f;

        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_StandingCharacterController; //character controller used for standing
        [SerializeField] private CharacterController m_CrouchCharacterController; //character controller used for crouching
        CharacterController currentCharController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;

        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private float m_cameraOriginalFOV;
        private AudioSource m_AudioSource;
        
        Coroutine crouchCoroutine = null;

        //for management related to objects that "get the player busy" - like computers and the diorama
        public delegate void InputEventDelegate();
        public static event InputEventDelegate ExitInteraction;

        //used for canvas elements management
        public delegate void FacingIconPromptDelegate(bool b);
        public static event FacingIconPromptDelegate FacingPromptIconEvent;
        public delegate void FacingTextPromptDelegate(string s);
        public static event FacingTextPromptDelegate FacingPromptTextEvent;
        
        private void Start()
        {
            QualitySettings.vSyncCount = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_StandingCharacterController = GetComponent<CharacterController>();
            currentCharController = m_StandingCharacterController;

            m_OriginalCameraPosition = mainCamera.transform.localPosition;
            m_cameraOriginalFOV = mainCamera.fieldOfView;
            m_FovKick.Setup(mainCamera);
            m_HeadBob.Setup(mainCamera, m_StepInterval);

            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;

            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, mainCamera.transform);
        }

        private void Update()
        {
            if (GameStateManager.gameState == (GameState.IN_GAME & GameState.IN_GAME_LOOK_ONLY))
            {
                RotateView();
                CheckActionInput();
            }

            if(GameStateManager.gameState == GameState.INTERACTING_W_ITEM)
            {
                if (Input.GetButton("Cancel"))
                    ExitInteraction();
            }

            if (m_IsCrouching) //makes sure that when moving the child controller the parent follows
                transform.position = m_CrouchCharacterController.transform.position;

            if (!m_PreviouslyGrounded && currentCharController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!currentCharController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = currentCharController.isGrounded;
        }

        private void FixedUpdate()
        {
            float speed = 0;
            if (GameStateManager.gameState == GameState.IN_GAME)
                CheckMovementInput(out speed);

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it

            Physics.SphereCast(transform.position, currentCharController.radius, Vector3.down, out RaycastHit hitInfo,
                               currentCharController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (currentCharController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                /* UNCOMMENT THIS TO ENABLE JUMP
                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
                */
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = currentCharController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

           // m_MouseLook.UpdateCursorLock();
        }

        void ManageMovementAcceleration(bool moving)
        {
            if (moving)
                acceleration = Mathf.Clamp(acceleration + Time.deltaTime * accelerationRate, 0.0f, 1.0f);
            else
                acceleration = 0.0f;       
        }

        #region Movement Related Methods
        private void CheckMovementInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");
            ManageMovementAcceleration(m_Input.sqrMagnitude != 0);

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            if (!m_IsCrouching)
                speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            else
                speed = m_WalkSpeed * m_CrouchSpeedModifier;

            speed *= acceleration;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
                m_Input.Normalize();

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && currentCharController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }

        }

        private void ProgressStepCycle(float speed)
        {
            if (currentCharController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (currentCharController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio(DetermineFloor());
        }

        private void ToggleCrouch() //crouches/stands
        {
            //check if possible or return
            if (m_IsCrouching) //if we are crouching check if there's anything above our head and if not then stand up
            {
                if (!CanStandUp())
                    return;

                m_IsCrouching = false;

                m_CrouchCharacterController.enabled = false;
                currentCharController = m_StandingCharacterController;
                currentCharController.enabled = true;

                if (crouchCoroutine != null)
                    StopCoroutine(crouchCoroutine);

                crouchCoroutine = StartCoroutine(PerformCrouch(mainCamera.transform.localPosition, m_OriginalCameraPosition));
            }

            else //crouch down
            {
                m_IsCrouching = true;

                m_StandingCharacterController.enabled = false;
                currentCharController = m_CrouchCharacterController;
                currentCharController.enabled = true;

                if (crouchCoroutine != null)
                    StopCoroutine(crouchCoroutine);

                crouchCoroutine = StartCoroutine(PerformCrouch(mainCamera.transform.localPosition, Vector3.zero));
            }
        }

        private IEnumerator PerformCrouch(Vector3 startPos, Vector3 endPos) //lerps camera according to whether crouching or standing
        {
            float lapsed = 0;

            while (lapsed < 1)
            {
                mainCamera.transform.localPosition = Vector3.Lerp(startPos, endPos, 1 - (1 - lapsed) * (1 - lapsed));
                lapsed += Time.deltaTime;
                yield return null;
            }
        }

        private bool CanStandUp() //checks for gameobjects that might block the stand action
        {
            Ray ray = new Ray(m_CeilingDetector.position, Vector3.up);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, 0.85f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != m_CrouchCharacterController.gameObject)
                    return false;
            }
            return true;
        }

        private void PlayFootStepAudio(AudioClip[] sounds)
        {
            if (!currentCharController.isGrounded || sounds == null)
                return;

            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, sounds.Length);
            m_AudioSource.clip = sounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            sounds[n] = sounds[0];
            sounds[0] = m_AudioSource.clip;
        }

        private AudioClip[] DetermineFloor() //checks what kind of floor we stepped on and returns an array of appropriate sounds to pick from
        {
            if (Physics.SphereCast(m_FloorDetector.position, .3f, Vector3.down, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.GetComponent<FloorType>() != null) //can be expanded to also work with say furniture
                {
                    switch (hit.collider.GetComponent<FloorType>().type)
                    {
                        case FloorType.FloorKind.CARPET:
                            return m_CarpetFootstepSounds;

                        case FloorType.FloorKind.WOOD:
                            return m_WoodFootstepSounds;

                        case FloorType.FloorKind.CONCRETE:
                            return m_ConcreteFootstepSounds;

                        default:
                            return null;
                    }
                }
            }
            return null;
        }

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        #endregion

        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition = Vector3.zero;

            if (!m_UseHeadBob || m_IsCrouching)
                return;

            if (currentCharController.velocity.magnitude > 0 && currentCharController.isGrounded)
            {
                mainCamera.transform.localPosition =
                    m_HeadBob.DoHeadBob(currentCharController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = mainCamera.transform.localPosition;
                newCameraPosition.y = mainCamera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            mainCamera.transform.localPosition = newCameraPosition;
        }

        public void SetRotation(Quaternion newRotation)
        {
            this.transform.rotation = newRotation;
            m_MouseLook.ForceRotation(newRotation);
        }
        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, mainCamera.transform);
        }

        private void CheckActionInput()
        {
            if (Input.GetMouseButtonDown(0)) //interacting with objects
            {
                CheckForInteractible();           
                CheckForViewport();
            }

            /* Not needed for now
            if (Input.GetKeyDown(KeyCode.LeftControl))
                ToggleCrouch();
            */

            CheckForPrompt();
            Zoom(Input.GetMouseButton(1));
        }

        private void CheckForViewport()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 1.9f))
            {
                if (hit.collider.GetComponent<ObjectOfInterest>() != null)
                    hit.collider.GetComponent<ObjectOfInterest>().FocusCamera();
            }
        }

        private void CheckForInteractible() //called when player clicks
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height /2));
            if (Physics.Raycast(ray, out RaycastHit hit, 1.9f))
            {
                //Debug.Log(hit.collider.GetComponent<IInteractable>());

                if (hit.collider.GetComponent<IInteractable>() != null)
                    hit.collider.GetComponent<IInteractable>().InteractWith();
            }
        }

        //checks if we are facing an object that emits a prompt and updates HUD accordingly
        private void CheckForPrompt() //called every time (so long as we are in IN_GAME state)
        {
            if (Camera.main == null) { return; }

            //cast ray from center of screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            //FOR PROMPT TEXT
            if (Physics.Raycast(ray, out RaycastHit hit, 1.9f))
            {
                if (hit.collider.GetComponent<ITextPrompt>() != null)
                    FacingPromptTextEvent?.Invoke(hit.collider.GetComponent<ITextPrompt>().PromptText());
                else
                    FacingPromptTextEvent?.Invoke("");
            }
            else if (Physics.SphereCast(ray.origin, 2, ray.direction, out hit, 2.0f))
            {
                if (hit.collider.GetComponent<SimplePromptObjectTextOnly>() != null)
                    FacingPromptTextEvent?.Invoke(hit.collider.GetComponent<ITextPrompt>().PromptText());
                else
                    FacingPromptTextEvent?.Invoke("");
            }
            else
                FacingPromptTextEvent?.Invoke("");

            //FOR PROMPT ICON
            if (Physics.Raycast(ray, out hit, 1.9f))
            {
                if (hit.collider.GetComponent<IInteractable>() != null)
                    FacingPromptIconEvent?.Invoke(true);
                else
                    FacingPromptIconEvent?.Invoke(false);
            }
            else
                FacingPromptIconEvent?.Invoke(false);
        }

        /// <summary>
        /// Pass true to zoom in, false to zoom out
        /// </summary>
        private void Zoom(bool inOut)
        {
            var val = inOut ? -1 : 1;

            mainCamera.fieldOfView += val;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, m_CameraZoomFOV, m_cameraOriginalFOV);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
                return;

            if (body == null || body.isKinematic)
                return;

            body.AddForceAtPosition(currentCharController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
