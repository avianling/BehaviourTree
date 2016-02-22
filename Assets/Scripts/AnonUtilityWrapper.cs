using UnityEngine;
using System.Collections;
using System;

public class AnonUtilityWrapper : Decorator, IUtilityBehaviour {

    public Func<AnonUtilityWrapper, float> utilityFunction;
    public Animal target;

    public AnonUtilityWrapper(string name = "") : base(name) { }

    public override string Description
    {
        get
        {
            return base.Description + ".\n Utility: " + Utility();
        }
    }

    public float Utility()
    {
        return utilityFunction(this);
    }

    public override void SetTarget(Animal animal)
    {
        target = animal;
        base.SetTarget(animal);
    }
}
