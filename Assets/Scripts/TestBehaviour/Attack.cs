using UnityEngine;
using System.Collections;
using System;

public class Attack : Behaviour
{
    public const float attackDistance = 1f;

    protected override BehaviourState RunStart()
    {
        if ( target.target != null )
        {
            return BehaviourState.Running;
        } else
        {
            return BehaviourState.Completed;
        }
    }

    protected override BehaviourState RunUpdate()
    {
        if ((target.target.GetPosition() - target.GetPosition()).sqrMagnitude < attackDistance)
        {
            Animal animalTarget = target.target as Animal;
            animalTarget.Killed(target as Animal);
            return BehaviourState.Completed;
        }
        else
        {
            return BehaviourState.Failed;
        }
    }
}
