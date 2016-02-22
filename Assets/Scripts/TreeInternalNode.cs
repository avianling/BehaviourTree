using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class TreeInternalNode : IBehaviour
{
    public List<IBehaviour> children = new List<IBehaviour>();

    // get the next behaviour to use. 
    // Returns null if finished.
    protected abstract IBehaviour Evaluate();
    protected IBehaviour lastBehaviour;
    protected BehaviourState lastState;

    private string name;
    public string Description
    {
        get
        {
            return name;
        }
    }

    public TreeInternalNode(string name)
    {
        this.name = name;
    }

    protected virtual void Reset()
    {
        // reset this in some way?
    }

    public BehaviourState LastState
    {
        get
        {
            return lastState;
        }
    }

    public virtual BehaviourState Start()
    {
        lastBehaviour = Evaluate();
        if ( lastBehaviour == null )
        {
            return BehaviourState.Completed;
        } else
        {
            BehaviourState state = lastBehaviour.Start();
            if ( state == BehaviourState.Completed )
            {
                lastBehaviour.End();
            }
            lastState = state;
            return state;
        }
    }

    public virtual BehaviourState Update()
    {
        IBehaviour newBehaviour = Evaluate();
        if ( newBehaviour == null )
        {
            lastState = BehaviourState.Completed;
            return BehaviourState.Completed;
        } else
        {
            if (newBehaviour != lastBehaviour)
            {
                lastBehaviour.End();
                lastBehaviour = newBehaviour;
                return newBehaviour.Start();
            } else
            {
                lastBehaviour = newBehaviour;
                BehaviourState state = newBehaviour.Update();
                if ( state == BehaviourState.Completed )
                {
                    newBehaviour.End();
                }
                lastState = state;
                return state;
            }
        }
    }

    public virtual void End()
    {
        if (lastBehaviour != null)
        {
            lastBehaviour.End();
        }
        lastState = BehaviourState.Unstarted;
    }

    public void SetTarget(Animal animal)
    {
        for ( int i=0; i < children.Count; i++)
        {
            children[i].SetTarget(animal);
        }
    }
}
