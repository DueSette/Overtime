using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpookyBalloonScript : MonoBehaviour
{
    [SerializeField] float freeWanderRadius, playerDetectionRadius, speed, idleTime;
    [SerializeField] AudioClip hoveringSound, chasingSound, explosionSound;

    #region Internal stuff
    private static UnityStandardAssets.Characters.FirstPerson.FirstPersonController player; //static ref to player - all hail nested namespaces
    AudioSource aud;

    private float downTimer;
    private Vector3 wanderSpot;
    private Vector3 startPos;

    private enum BalloonState { IDLE, WANDERING, CHASING };
    private BalloonState state = BalloonState.IDLE;
    #endregion

    private void OnEnable()
    {
        if (player == null)
            player = FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        aud = GetComponent<AudioSource>();
        downTimer = idleTime;
        startPos = transform.position;
    }

    private void Update()
    {
        HandleState();     
    }

    void HandleState()
    {
        switch(state)
        {
            case BalloonState.IDLE:
                {
                    downTimer -= Time.deltaTime;
                    if (downTimer <= 0.0f)
                        ChangeState(BalloonState.WANDERING);

                    if (IsPlayerNear())
                        ChangeState(BalloonState.CHASING);
                    break;
                }
            case BalloonState.WANDERING:
                {
                    Vector3 dir = (wanderSpot - transform.position).normalized;
                    transform.position += dir * (speed / 3.0f) * Time.deltaTime;

                    if (ReachedWanderSpot())
                        ChangeState(BalloonState.IDLE);

                    if (IsPlayerNear())
                        ChangeState(BalloonState.CHASING);
                    break;
                }
            case BalloonState.CHASING:
                {
                    Vector3 dir = (player.transform.position - transform.position).normalized;
                    transform.position += dir * speed * Time.deltaTime;

                    if (!IsPlayerNear())
                        ChangeState(BalloonState.IDLE);
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
                    downTimer = idleTime;
                    break;
                }
            case BalloonState.WANDERING:
                {
                    Vector2 w = PickWanderSpot();
                    wanderSpot.x = w.x;
                    wanderSpot.y = transform.position.y;
                    wanderSpot.z = w.y;                   
                    break;
                }
            case BalloonState.CHASING:
                {
                    //TODO: play "chasing" clip
                    break;
                }
        }
    }

    private bool IsPlayerNear()
    {
        float dist = (player.transform.position - transform.position).sqrMagnitude;
        return (dist < playerDetectionRadius * playerDetectionRadius);
    }

    private Vector2 PickWanderSpot()
    {
        Vector3 result;
        result = startPos + Random.insideUnitSphere * freeWanderRadius;

        return new Vector2(result.x, result.z);
    }

    private bool ReachedWanderSpot()
    {
        float f = Mathf.Floor(transform.position.x);
        float g = Mathf.Floor(transform.position.z);
        float a = Mathf.Floor(wanderSpot.x);
        float b = Mathf.Floor(wanderSpot.z);

        return (f == a) && (g == b);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //explode on player contact
    }
}
