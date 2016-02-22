using UnityEngine;
using System.Collections;

public class AnimalRelationships : MonoBehaviour {

    private static AnimalRelationships singleton;

    public static AnimalRelationships Instance
    {
        get
        {
            return singleton;
        }
    }

    void Awake()
    {
        singleton = this;
    }

    public AnimalRelationship[] relationships;

    public AnimalRelationship GetOpinionOf(AnimalTypes source, AnimalTypes target )
    {
        for ( int i=0; i < relationships.Length; i++ )
        {
            if ( relationships[i].source == source && relationships[i].target == target )
            {
                return relationships[i];
            }
        }

        return new AnimalRelationship();
    }

    public void AnimalWasKilled( Animal killer, Animal victim )
    {
        AnimalRelationship relationship = AnimalRelationships.Instance.GetOpinionOf(victim.type, killer.type);
        // increase how much we fear the thing which just killed us.
        relationship.fear += 0.02f;

        // Increase the hatred of the killed by those who love us.
        for ( int i=0; i < relationships.Length; i++ )
        {
            if ( relationships[i].target == victim.type )
            {
                if ( relationships[i].love > 0 )
                {
                    GetOpinionOf(relationships[i].source, killer.type).hate += 0.05f * relationships[i].love;
                }
            }
        }
    }   

}
