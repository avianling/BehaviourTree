using UnityEngine;
using System.Collections;
using System;

public class WalkToTarget : WalkToBehaviour
{
    public WalkToTarget(string name ="") : base(name) { }

    protected override BehaviourState RunStart()
    {
        target.targetPosition = target.target.GetPosition();
        return base.RunStart();
    }

    protected override BehaviourState RunUpdate()
    {
        if (target.target == null || !target.target.IsAlive() )
        {
            return BehaviourState.Failed;
        }
        else
        {
            target.targetPosition = target.target.GetPosition();
            return base.RunUpdate();
        }
    }
}
