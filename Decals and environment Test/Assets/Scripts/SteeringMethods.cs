using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringMethods
{
    public static Vector3 Seek(Transform agent, Transform quarry, float maxSpeed)
    {
        Vector3 desiredMovement = Vector3.Normalize(quarry.position - agent.position) * maxSpeed;
        Vector3 steering = desiredMovement - (agent.GetComponent<Rigidbody>().velocity);
        return steering;
    }

    public static Vector2 Seek2D(Transform agent, Transform quarry, float maxSpeed)
    {
        Vector2 desiredMovement = Vector3.Normalize(quarry.position - agent.position) * maxSpeed;
        Vector2 steering = desiredMovement - (agent.GetComponent<Rigidbody2D>().velocity);
        return steering;
    }

    public static Vector3 Flee(Transform agent, Transform quarry, float maxSpeed)
    {
        Vector3 desiredMovement = Vector3.Normalize(agent.position - quarry.position) * maxSpeed;
        Vector3 steering = desiredMovement - (agent.GetComponent<Rigidbody>().velocity);
        return steering;
    }

    public static Vector2 Flee2D(Transform agent, Transform quarry, float maxSpeed)
    {
        Vector2 desiredMovement = Vector3.Normalize(agent.position - quarry.position) * maxSpeed;
        Vector2 steering = desiredMovement - (agent.GetComponent<Rigidbody2D>().velocity);
        return steering;
    }

    public static Vector3? Arrival(Transform agent, Transform quarry, float maxSpeed, float arrivalRadius)
    {
        Vector3 desiredMovement = quarry.position - agent.position;
        float distance = desiredMovement.magnitude;

        Vector3 steering;
        if (distance > 0)
        {
            desiredMovement.Normalize();
            if (distance < arrivalRadius)
                desiredMovement *= maxSpeed * (distance / arrivalRadius);
            else
                desiredMovement *= maxSpeed;

            steering = desiredMovement - agent.GetComponent<Rigidbody>().velocity;
            return steering;
        }
        return null;
    }

    public static Vector2? Arrival2D(Transform agent, Transform quarry, float maxSpeed, float arrivalRadius)
    {
        Vector2 desiredMovement = quarry.position - agent.position;
        float distance = desiredMovement.magnitude;

        Vector2 steering;
        if (distance > 0)
        {
            desiredMovement.Normalize();
            if (distance < arrivalRadius)
                desiredMovement *= maxSpeed * (distance / arrivalRadius);
            else
                desiredMovement *= maxSpeed;

            steering = desiredMovement - agent.GetComponent<Rigidbody2D>().velocity;
            return steering;
        }
        return null;
    }

    public static Vector3? Departure(Transform agent, Transform quarry, float maxSpeed, float departureRadius)
    {
        Vector3 desiredMovement = agent.position - quarry.position;
        float distance = desiredMovement.magnitude;

        Vector3 steering;
        if (distance > 0)
        {
            desiredMovement.Normalize();
            if (distance < departureRadius) //if in the circle of departure
                desiredMovement *= maxSpeed * (departureRadius / distance);
            else
                desiredMovement *= maxSpeed;

            steering = desiredMovement - agent.GetComponent<Rigidbody>().velocity;
            return steering;
        }
        return null;
    }

    public static Vector2? Departure2D(Transform agent, Transform quarry, float maxSpeed, float departureRadius)
    {
        Vector2 desiredMovement = agent.position - quarry.position;
        float distance = desiredMovement.sqrMagnitude;

        Vector2 steering;
        if (distance > 0)
        {
            desiredMovement.Normalize();
            if (distance < departureRadius)
                desiredMovement *= maxSpeed * (departureRadius / distance);
            else
                desiredMovement *= maxSpeed;

            steering = desiredMovement - agent.GetComponent<Rigidbody2D>().velocity;
            return steering;
        }
        return null;
    }

    public static Vector3 Pursue(Transform agent, Transform quarry, float maxSpeed)
    {
        float predictionMargin = (agent.position - quarry.position).magnitude / maxSpeed;
        Vector3 predictedVelocity = quarry.GetComponent<Rigidbody>().velocity * predictionMargin;
        Vector3 predictedQuarryPos = quarry.position + predictedVelocity;

        Vector3 steering = InternalSeek(agent, predictedQuarryPos, maxSpeed);
        return steering;

        Vector3 InternalSeek(Transform internalAgent, Vector3 predictedQuarryPosition, float internalMaxSpeed)
        {
            Vector3 internalDesiredMovement = Vector3.Normalize(predictedQuarryPosition - internalAgent.position) * internalMaxSpeed;
            Vector3 internalSteering = internalDesiredMovement;
            return internalSteering;
        }
    }

    public static Vector2 Pursue2D(Transform agent, Transform quarry, float maxSpeed)
    {
        float predictionMargin = (agent.position - quarry.position).magnitude / maxSpeed;
        Vector2 predictedVelocity = quarry.GetComponent<Rigidbody2D>().velocity * predictionMargin;
        Vector2 predictedQuarryPos = (Vector2)quarry.position + predictedVelocity;

        Vector2 steering = InternalSeek2D(agent, predictedQuarryPos, maxSpeed);
        return steering;

        Vector2 InternalSeek2D(Transform internalAgent, Vector2 predictedQuarryPosition, float internalMaxSpeed)
        {
            Vector2 internalDesiredMovement = Vector3.Normalize(predictedQuarryPosition - (Vector2)internalAgent.position) * internalMaxSpeed;
            Vector2 internalSteering = internalDesiredMovement;
            return internalSteering;
        }
    }
}
