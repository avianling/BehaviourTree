using UnityEngine;
using System.Collections.Generic;
using System;

public class FindFood : FindTarget
{
    public FindFood(string name) : base(name) { }

    public override bool IsTargettable(ITargettable other)
    {
        Animal animal = other as Animal;
        if ( animal == null || animal.type != AnimalTypes.Shepard)
        {
            return base.IsTargettable(other);
        } else
        {
            return false;
        }
    }

    public override IListConverter<ITargettable> GetAllPotentialTargets()
    {
        if (target.predator) {
            ListConverter<Animal,ITargettable> lookup = new ListConverter<Animal, ITargettable>(AnimalMap.animalMap.animals);
            return lookup;
        } else
        {
            return new ListConverter<Carrot, ITargettable>(AnimalMap.animalMap.carrots);
        }
    }
}
