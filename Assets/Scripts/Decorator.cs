using UnityEngine;
using System.Collections;
using System;

public abstract class Decorator : IBehaviour {
    public IBehaviour child;

    private BehaviourState lastState;

    public BehaviourState LastState
    {
        get
        {
            return lastState;
        }
    }

    private string name;
    public virtual string Description
    {
        get
        {
            return name;
        }
    }

    public Decorator(string name="")
    {
        this.name = name;
    }

    public IBehaviour SetChild(IBehaviour child)
    {
        this.child = child;
        return child;
    }

    public virtual void SetTarget(Animal animal)
    {
        child.SetTarget(animal);
    }

    public virtual BehaviourState Start()
    {
        lastState = child.Start();
        return lastState;
    }
    public virtual BehaviourState Update()
    {
        lastState = child.Update();
        return lastState;
    }
    public virtual void End() {
        lastState = BehaviourState.Unstarted;
        child.End();
    }
}
