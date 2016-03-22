using UnityEngine;
using System.Collections;

public abstract class Behaviour : IBehaviour {
    public Animal target;
    private BehaviourState lastState;

    private string name;
    public string Description
    {
        get
        {
            return name;
        }
    }

    public Behaviour(string name = "")
    {
        this.name = name;
    }

    public void SetTarget(Animal animal)
    {
        this.target = animal;
    }

    public BehaviourState LastState
    {
        get
        {
            return lastState;
        }
    }

    protected abstract BehaviourState RunStart();
    public BehaviourState Start()
    {
        lastState = RunStart();
        return lastState;
    }

    protected abstract BehaviourState RunUpdate();
    public BehaviourState Update()
    {
        lastState = RunUpdate();
        return lastState;
    }

    public virtual void End() {
        lastState = BehaviourState.Unstarted;
    }
}
