using UnityEngine;
using System.Collections;
using System;

public class Breed : Behaviour
{
    public Breed(string name) : base(name) { }

    public bool CanBreed( ITargettable other )
    {
        Animal animal = other as Animal;
        if ( animal != null )
        {
            return animal.type == target.type;
        } else
        {
            return false;
        }
    }

    public bool InRange()
    {
        return (target.target.GetPosition() - target.GetPosition()).magnitude < 1f;
    }

    protected override BehaviourState RunStart()
    {
        if ( CanBreed(target.target) && InRange() )
        {
            AnimalFactory.Instance.CreateAnimal(target.type).transform.position = target.transform.position;
            target.hunger += 15;
            return BehaviourState.Completed;
        } else
        {
            return BehaviourState.Failed;
        }
    }

    protected override BehaviourState RunUpdate()
    {
        return BehaviourState.Completed;
    }
}
