using UnityEngine;
using System.Collections;
using System;

public class Repeater : Decorator {
    public override BehaviourState Start()
    {
        return child.Start();
    }

    public override BehaviourState Update()
    {
        BehaviourState childState = child.Update();
        if ( childState == BehaviourState.Completed || childState == BehaviourState.Failed )
        {
            child.Start();
        }
        return BehaviourState.Running;
    }
}
