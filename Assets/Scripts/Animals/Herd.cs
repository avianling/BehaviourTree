using UnityEngine;
using System.Collections.Generic;

// contains a list of animals.
public class Herd {
    public AnimalTypes type;
    public List<Animal> herdMembers;
    public const int maxSize = 5;

    public Herd()
    {
        herdMembers = new List<Animal>(maxSize);
    }

    public Vector3 GetHerdCenter()
    {
        Vector3 pos = Vector3.zero;

        for ( int i=0; i < herdMembers.Count; i++ )
        {
            pos += herdMembers[i].GetPosition();
        }

        return pos / herdMembers.Count;
    }

    public void AddMember( Animal member )
    {
        herdMembers.Add(member);
        if ( herdMembers.Count >= maxSize )
        {
            AnimalMap.animalMap.HerdIsFull(this);
        }
    }

    public void RemoveMember(Animal member)
    {
        herdMembers.Remove(member);
        AnimalMap.animalMap.HerdIsAvailable(this);
    }
}
