using UnityEngine;
using System.Collections;

public abstract class CachedTreeInternalNode : TreeInternalNode {
    public CachedTreeInternalNode(string name ) : base(name)
    {

    }

    public override BehaviourState Start()
    {
        Reset();

        // Run this behaviour.
        // If it's complete, go through the remaining behaviours according to Evaluate until we find one which isn't complete, or we run out.
        lastBehaviour = Evaluate();
        lastState = StartUntilRunning(lastBehaviour);
        return lastState;
    }

    // Starting from the given behaviour, work through all of them until you find either
    // 1) A behaviour which is running
    // 2) an error,
    // 3) run out of behaviours.
    protected BehaviourState StartUntilRunning(IBehaviour startingBehaviour)
    {
        lastBehaviour = startingBehaviour;
        if (lastBehaviour == null)
        {
            return BehaviourState.Completed;
        }

        BehaviourState currentState;
        while (true)
        {
            currentState = lastBehaviour.Start();
            if (currentState == BehaviourState.Completed)
            {
                lastBehaviour.End();
                // Try to get the next behaviour.
                IBehaviour nextBehaviour = Evaluate();
                if (nextBehaviour == null)
                {
                    return BehaviourState.Completed;
                }
                else
                {
                    lastBehaviour = nextBehaviour;
                }
            }
            else
            {
                return currentState;
            }
        }
    }

    public override BehaviourState Update()
    {
        BehaviourState currentState;

        if (lastBehaviour == null)
        {
            lastState = BehaviourState.Completed;
            return BehaviourState.Completed;
        }

        currentState = lastBehaviour.Update();
        if (currentState == BehaviourState.Completed)
        {
            lastBehaviour.End();
            lastState = StartUntilRunning(Evaluate());
            return lastState;
        }
        else
        {
            lastState = currentState;
            return currentState;
        }
    }
}
