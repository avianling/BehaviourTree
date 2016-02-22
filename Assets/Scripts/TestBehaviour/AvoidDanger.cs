using System.Collections.Generic;
using System;
using UnityEngine;

// Find the best place to go to avoid being in danger.
public class AvoidDanger : Behaviour
{
    public AvoidDanger(string name) : base(name) { }

    private bool IsAnimalDangerous( Animal animal )
    {
        AnimalRelationship relationship = AnimalRelationships.Instance.GetOpinionOf(target.type, animal.type);
        return relationship.fear > 0f;
    }

    public List<Animal> GetDangerSources()
    {
        List<Animal> dangerSources = new List<Animal>();
        List<Animal> potentialDangerSources = AnimalMap.animalMap.animals;

        HashSet<AnimalTypes> dangerousTypes = new HashSet<AnimalTypes>();
        HashSet<AnimalTypes> ignoreType = new HashSet<AnimalTypes>();

        for ( int i=0; i < potentialDangerSources.Count; i++ )
        {
            if ( ignoreType.Contains(potentialDangerSources[i].type) )
            {
                continue;
            } else
            {
                if ( dangerousTypes.Contains(potentialDangerSources[i].type) )
                {
                    dangerSources.Add(potentialDangerSources[i]);
                    continue;
                } else
                {
                    // We don't know what to do with this, work out what to do.
                    if ( IsAnimalDangerous(potentialDangerSources[i] ) )
                    {
                        dangerousTypes.Add(potentialDangerSources[i].type);
                        dangerSources.Add(potentialDangerSources[i]);
                    } else
                    {
                        ignoreType.Add(potentialDangerSources[i].type);
                    }
                }
            }
        }

        return dangerSources;        
    }

    public Vector3 GetAvoidanceVector( List<Animal> toAvoid )
    {
        Vector3 avoidanceVector = Vector3.zero;

        for ( int i=0; i < toAvoid.Count; i++ )
        {
            Vector3 toOther = toAvoid[i].transform.position - target.transform.position;
            float length = toOther.magnitude;
            if ( length < 6 )
            {
                // consider it.
                toOther /= length;
                // Inverse avoidance. Avoid closer things more!
                toOther *= Mathf.Pow((6f - length), 2);
                avoidanceVector -= toOther;
            }
        }

        avoidanceVector.Normalize();
        return avoidanceVector;
    }

    protected override BehaviourState RunStart()
    {
        Vector3 avoidanceVector = GetAvoidanceVector(GetDangerSources());
        target.targetPosition = target.transform.position + avoidanceVector * 8f;
        //Debug.DrawLine(target.transform.position, target.targetPosition, Color.red, 5f);
        return BehaviourState.Completed;
    }

    protected override BehaviourState RunUpdate()
    {
        return BehaviourState.Completed;
    }
}
