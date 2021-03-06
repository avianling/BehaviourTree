﻿using UnityEngine;
using System.Collections;
using System;

public class Eat : Behaviour
{
    public Eat(string name) : base(name) { }

    public bool CanEat( Animal target, ITargettable victim )
    {
        if ( target.predator )
        {
            return victim is Animal;
        } else
        {
            return victim is Carrot;
        }
    }

    protected override BehaviourState RunStart()
    {
        // do we have something to eat?
        // if we don't, or we've targetted something we cant eat, return failure.
        if ( CanEat(target, target.target))
        {
            target.eating = true;
            return BehaviourState.Running;
        } else
        {
            return BehaviourState.Failed;
        }
    }

    protected override BehaviourState RunUpdate()
    {
        // Take food from the source!
        if ( target.predator )
        {
            Animal hunted = target.target as Animal;
            hunted.Killed(target);
            target.timesEaten++;
            target.hunger = 0;
            return BehaviourState.Completed;
        } else
        {
            Carrot source = target.target as Carrot;
            target.hunger = 0;
            source.Die();
            target.timesEaten++;
            return BehaviourState.Completed;
        }
    }
    
}
