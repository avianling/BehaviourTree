using UnityEngine;
using System.Collections.Generic;

public class WalkToBehaviour : Behaviour
{
    BehaviourState state = BehaviourState.Unstarted;
    protected Vector3 initialPosition;

    public const float lookaheadDistance = 0.5f;
    public const float intimidationRange = 15;
    public const float avoidanceStrength = 3f;

    public WalkToBehaviour(string name) : base(name) { }

    public BehaviourState GetState()
    {
        return state;
    }

    protected override BehaviourState RunStart()
    {
        target.transform.LookAt(target.targetPosition);
        return BehaviourState.Running;
    }

    private bool NeedToAvoid(Animal other)
    {
        if ( other == target )
        {
            return false;
        }
        if ( other.type == target.type )
        {
            return target.loneWolf;
        }        
        Vector3 toOther = other.GetPosition() - target.GetPosition();
        if (Mathf.Abs(toOther.x) < intimidationRange && Mathf.Abs(toOther.z) < intimidationRange )
        {
            // how afraid are we of them?
            // If we are more afraid of them then we love them, we need to avoid them in this case.
            AnimalRelationship relationship = AnimalRelationships.Instance.GetOpinionOf(target.type, other.type);
            if ( relationship.fear > relationship.love )
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            // far enough away that we dont need to avoid them.
            return false;
        }
    }

    private Vector3 GetAvoidanceVector()
    {
        // if anything is within a certain distance ( say 4 units ) // apply an avoidance vector.
        Vector3 lookahead = target.transform.position + target.transform.forward * lookaheadDistance;

        List<Animal> animals = AnimalMap.animalMap.animals;
        Vector3 combinedAvoidance = Vector3.zero;
        int count = 0;

        for ( int i=0; i < animals.Count; i++ )
        {
            if ( NeedToAvoid(animals[i]) )
            {
                combinedAvoidance += AvoidTarget(animals[i], lookahead);
                count++;
            }
        }

        if (count != 0)
        {
            combinedAvoidance /= count;
        }
        //Debug.DrawRay(target.GetPosition(), combinedAvoidance, Color.red);
        return combinedAvoidance;
    }

    private Vector3 AvoidTarget(Animal other, Vector3 lookahead)
    {
        Vector3 avoidanceVector = (lookahead - other.GetPosition());
        float length = avoidanceVector.magnitude;
        avoidanceVector /= length;

        float fearStrength = 3f * AnimalRelationships.Instance.GetOpinionOf(target.type, other.type).fear;

        // stronger as it gets closer.
        return avoidanceVector * fearStrength * ((intimidationRange - length) / intimidationRange);
    }

    protected override BehaviourState RunUpdate()
    {
        Vector3 toTarget = target.targetPosition - target.transform.position;
        float distance = toTarget.magnitude;
        if ( distance < 0.05f )
        {
            target.transform.position = target.targetPosition;
            return BehaviourState.Completed;
        } else
        {
            Vector3 flattenedTarget = target.targetPosition;
            flattenedTarget.y = target.GetPosition().y;
            target.transform.LookAt(flattenedTarget);

            Vector3 avoidanceVector = GetAvoidanceVector();

            // work out if the avoidance vector is too close to the current direction.
            Vector3 avoidanceDirection = avoidanceVector.normalized;
            if ( Vector3.Dot(target.transform.forward, avoidanceDirection) < -0.9f )
            {
                return BehaviourState.Failed;
            } else
            {
                if (target.tag == "Debug")
                {
                    Debug.DrawRay(target.transform.position, target.transform.forward, Color.red);
                    Debug.DrawRay(target.transform.position, avoidanceDirection, Color.blue);
                    Debug.Log("overlay of avoidance vector onto current direction is " + Vector3.Dot(target.transform.forward, avoidanceDirection));
                }
            }

            Vector3 direction = target.transform.forward + (avoidanceVector * avoidanceStrength);
            direction.Normalize();
            target.transform.position += direction * Time.deltaTime * target.speed;
            return BehaviourState.Running;
        }
    }
}
