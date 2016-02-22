using UnityEngine;
using System.Collections.Generic;

public class PickSameSpecies : Behaviour {
    ITargettable food;

    public PickSameSpecies(string name) : base(name) { }

    public ITargettable GetClosestSameSpecies()
    {
        List<Animal> potentialMates = AnimalMap.animalMap.animals;

        ITargettable closestBreeder = null;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < potentialMates.Count; i++)
        {
            if ( potentialMates[i].type == target.type && potentialMates[i] != target )
            {
                continue;
            }

            float distance = (potentialMates[i].GetPosition() - target.transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestBreeder = potentialMates[i];
            }
        }

        return closestBreeder;
    }

    protected override BehaviourState RunStart()
    {
        food = GetClosestSameSpecies();
        if (food == null)
        {
            return BehaviourState.Running;
        }
        else
        {
            target.target = food;
            return BehaviourState.Completed;
        }
    }

    protected override BehaviourState RunUpdate()
    {
        if (food != null)
        {
            return BehaviourState.Completed;
        }
        else
        {
            food = GetClosestSameSpecies();
            if (food != null)
            {
                target.target = food;
                return BehaviourState.Completed;
            }
            else
            {
                return BehaviourState.Running;
            }
        }
    }
}
