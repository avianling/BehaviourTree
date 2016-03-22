using UnityEngine;
using System.Collections.Generic;
using System;

public class FindHatedTarget : FindTarget {

    private Dictionary<AnimalTypes, float> hatedTypes = new Dictionary<AnimalTypes, float>();

    public FindHatedTarget(string name) : base(name) { }

    protected override float maxRange
    {
        get
        {
            return 20f;
        }
    }

    private void CacheHatedTypes()
    {
        hatedTypes.Clear();
        foreach ( AnimalTypes type in (AnimalTypes[])System.Enum.GetValues(typeof(AnimalTypes)) ) {
            AnimalRelationship relationship = AnimalRelationships.Instance.GetOpinionOf(target.type, type);
            hatedTypes.Add(type, relationship.hate);
        }
    }

    public override IListConverter<ITargettable> GetAllPotentialTargets()
    {
        IListConverter<ITargettable> list = new ListConverter<Animal, ITargettable>(AnimalMap.animalMap.animals);
        return list;
    }

    public override bool IsTargettable(ITargettable other)
    {
        return base.IsTargettable(other) && hatedTypes.ContainsKey(((Animal)other).type) && hatedTypes[((Animal)other).type] > 0;
    }

    public override float GetTargetScore(ITargettable other)
    {
        float value = 0f;
        hatedTypes.TryGetValue(((Animal)other).type, out value);
        return base.GetTargetScore(other) * hatedTypes[((Animal)other).type] * 10f;
    }

    public float GetUtility()
    {
        // returns a range of 0 - 1 based upon the proximity of a target & how much we hate them.
        CacheHatedTypes();

        IListConverter<ITargettable> allTargets = GetAllPotentialTargets();

        float highestScore = float.MinValue;
        Vector3 ourPosition = target.GetPosition();

        for ( int i=0; i < allTargets.Count; i++ )
        {
            if ( IsTargettable(allTargets[i]) )
            {
                float score = GetTargetScore(allTargets[i]);
                if ( score > highestScore )
                {
                    highestScore = score;
                }
            }
        }

        return highestScore;
    }

}
