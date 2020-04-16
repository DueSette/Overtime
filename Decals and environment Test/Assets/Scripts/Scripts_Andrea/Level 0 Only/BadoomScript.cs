using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(AudioSource))]
public class BadoomScript : MonoBehaviour
{
    [Header("Designer Friendly Options")]
    [SerializeField] bool fixedWanderer;
    [SerializeField, Tooltip("The first stop will always be the badoom's starting position. That means the first value of this array will be the SECOND stop and so on")]
    Vector3[] fixedWanderSpots;
    [SerializeField] float freeWanderRadius, playerDetectionRadius, maxSpeed, idleTime, turningSpeed = 0.08f;

    [SerializeField] AudioClip hoveringSound, chasingSound, explosionSound;
    [SerializeField] VisualEffect visualEffect;

    #region Internal fields
    private static UnityStandardAssets.Characters.FirstPerson.FirstPersonController player; //static ref to player - all hail nested namespaces
    NavMeshAgent balloonAI;

    AudioSource aud;
    private float idleTimer, chaseTimer;
    private Vector3 wanderSpot;

    private Vector3 startPos;
    private int fixedWandererIterator = 1; //determines the current node the fixed patroller is supposed to visit
    bool forwardWanderOrder = true; //for fixed patrolling logic

    private enum BalloonState { IDLE, WANDERING, CHASING };
    private BalloonState state = BalloonState.IDLE;
    #endregion

    private void OnEnable()
    {
        if (player == null)
            player = FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        aud = GetComponent<AudioSource>();
        aud.clip = hoveringSound;
        aud.Play();

        balloonAI = GetComponent<NavMeshAgent>();
        balloonAI.angularSpeed = 0;

        idleTimer = idleTime;
        startPos = transform.position;

        Gizmos.color = Color.cyan;
    }

    private void Update()
    {
        HandleState();
    }

    void HandleState()
    {
        switch (state)
        {
            case BalloonState.IDLE:
                {
                    idleTimer -= Time.deltaTime;
                    if (idleTimer <= 0.0f)
                        ChangeState(BalloonState.WANDERING);

                    if (IsPlayerNear())
                        ChangeState(BalloonState.CHASING);
                    break;
                }
            case BalloonState.WANDERING:
                {
                    balloonAI.SetDestination(wanderSpot);

                    BalloonLookAt(wanderSpot);

                    if (ReachedWanderSpot())
                        ChangeState(BalloonState.IDLE);

                    if (IsPlayerNear())
                        ChangeState(BalloonState.CHASING);
                    break;
                }
            case BalloonState.CHASING:
                {
                    balloonAI.speed = Mathf.Lerp(0.1f, maxSpeed, Mathf.Clamp(chaseTimer / maxSpeed, 0, 1));
                    balloonAI.SetDestination(player.transform.position);

                    chaseTimer += Time.deltaTime;

                    BalloonLookAt(player.transform);

                    if (!IsPlayerNear()) //sends badoom to its first spawnpoint
                        ResetActivity();
                    break;
                }
        }
    }

    void ChangeState(BalloonState state)
    {
        this.state = state;

        switch (state)
        {
            case BalloonState.IDLE:
                {
                    aud.clip = hoveringSound;
                    aud.Play();
                    idleTimer = idleTime;
                    break;
                }
            case BalloonState.WANDERING:
                {
                    PickWanderSpot();
                    balloonAI.speed = (maxSpeed / 3.0f);

                    break;
                }
            case BalloonState.CHASING:
                {
                    aud.clip = chasingSound;
                    aud.Play();
                    chaseTimer = 0.0f;
                    break;
                }
        }
    }

    private bool IsPlayerNear()
    {
        float dist = (player.transform.position - transform.position).sqrMagnitude;
        return (dist < playerDetectionRadius * playerDetectionRadius);
    }

    private void PickWanderSpot()
    {
        Vector3 spot;
        if (fixedWanderer)
        {
            spot = (fixedWandererIterator == 0) ? startPos : fixedWanderSpots[fixedWandererIterator - 1];
            fixedWandererIterator += forwardWanderOrder ? 1 : -1;

            if (fixedWandererIterator > fixedWanderSpots.Length - 1)
            {
                fixedWandererIterator = fixedWanderSpots.Length - 1;
                forwardWanderOrder = !forwardWanderOrder;
            }
            else if (fixedWandererIterator < 0)
            {
                fixedWandererIterator = 1;
                forwardWanderOrder = !forwardWanderOrder;
            }

            wanderSpot = spot;
        }
        else
        {
            spot = startPos + (Random.insideUnitSphere * freeWanderRadius);

            wanderSpot.x = spot.x;
            wanderSpot.y = transform.position.y;
            wanderSpot.z = spot.y;
        }
    }

    private bool ReachedWanderSpot()
    {
        float f = Mathf.Floor(transform.position.x);
        float g = Mathf.Floor(transform.position.y);
        float h = Mathf.Floor(transform.position.z);

        float a = Mathf.Floor(wanderSpot.x);
        float b = Mathf.Floor(wanderSpot.y);
        float c = Mathf.Floor(wanderSpot.z);

        return (f == a) && (g == b) && (h == c);
    }

    private void BalloonLookAt(Transform target)
    {
        Vector3 prev = transform.rotation.eulerAngles;
        transform.LookAt(target);
        Vector3 fut = transform.localRotation.eulerAngles;

        Vector3 v = Vector3.Lerp(prev, fut, turningSpeed);
        transform.rotation = Quaternion.Euler(prev.x, v.y, prev.z);
    }

    private void BalloonLookAt(Vector3 target)
    {
        Vector3 prev = transform.rotation.eulerAngles;
        transform.LookAt(target);
        Vector3 fut = transform.localRotation.eulerAngles;

        Vector3 v = Vector3.Lerp(prev, fut, turningSpeed);
        transform.rotation = Quaternion.Euler(prev.x, v.y, prev.z);
    }

    private void Pop() //badoom behaviour for explosion
    {
        //this means screen fx, sound, maybe camera shake, maybe slowed speed
       // visualEffect.gameObject.transform.parent = null;
       // visualEffect.Play();
        //visualEffect.gameObject.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        
        Destroy(gameObject);
    }

    private void ResetActivity() //back to square one and in wandering state
    {
        ChangeState(BalloonState.WANDERING);
        wanderSpot = startPos;
    }

    private void OnTriggerEnter(Collider coll)
    {
        switch (coll.GetComponent<Collider>().tag)
        {
            case "BalloonStopper":
                {
                    ResetActivity();
                    print("Badoom hit Wall!");
                }
                break;

            case "Player":
                {
                    Pop();
                    print("Badoom popped");
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!fixedWanderer) { return; }

        for (int i = 0; i < fixedWanderSpots.Length; i++)
            Gizmos.DrawWireSphere(fixedWanderSpots[i], 0.15f);
    }
}