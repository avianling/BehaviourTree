using UnityEngine;
using System.Collections;

public abstract class FindTarget : Behaviour {
    ITargettable food;
    
    protected virtual float maxRange
    {
        get
        {
            return 50f;
        }
    }

    public FindTarget(string name) : base(name) { }

    public virtual bool IsTargettable( ITargettable other )
    {
        return other.CanBeTargetted(target);
    }

    // How desperate is this animal to do this action?
    // if desperation > fear, ignore all fear.
    protected virtual float GetDesperation()
    {
        return 0f;
    }

    private float GetFearInfluence( ITargettable other )
    {
        // Based upon the proximity of this target to things we fear, pick another target.
        // fear should be overridden by desperation?
        // how should this be represented?
        // 1f = no fear modifier.
        float fear = 1f - AnimalMap.animalMap.GetFearOfPosition(target.type, other.GetPosition());

        if (fear < GetDesperation())
        {
            return 1f;
        } else
        {
            return fear;
        }
    }

    public virtual float GetTargetScore( ITargettable other )
    {
        float dist = (other.GetPosition() - target.transform.position).magnitude;
        if (dist > 0)
        {
            return ((maxRange - dist)/ maxRange) * GetFearInfluence(other);
        } else
        {
            return GetFearInfluence(other);
        }
    }

    public abstract IListConverter<ITargettable> GetAllPotentialTargets();

    public ITargettable GetBestTarget()
    {
        IListConverter<ITargettable> allTargets = GetAllPotentialTargets();

        ITargettable currentBest = null;
        float bestValue = 0f;

        for ( int i=0; i < allTargets.Count; i++ )
        {
            if ( IsTargettable(allTargets[i]) )
            {
                float value = GetTargetScore(allTargets[i]);
                if ( value > bestValue )
                {
                    currentBest = allTargets[i];
                    bestValue = value;
                }
            }
        }

        return currentBest;

        //List<Carrot> carrots = AnimalMap.animalMap.carrots;
        /*IListConverter<ITargettable> potentialFood = null;
        if (!target.predator)
        {
            potentialFood = new ListConverter<Carrot, ITargettable>(AnimalMap.animalMap.carrots);
        }
        else
        {
            potentialFood = new ListConverter<Animal, ITargettable>(AnimalMap.animalMap.animals);
        }


        ITargettable closestCarrot = null;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < potentialFood.Count; i++)
        {
            if (!potentialFood[i].CanBeTargetted(target))
            {
                continue;
            }

            float distance = (potentialFood[i].GetPosition() - target.transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestCarrot = potentialFood[i];
            }
        }

        return closestCarrot;*/
    }

    protected override BehaviourState RunStart()
    {
        food = GetBestTarget();
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
            food = GetBestTarget();
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
