using UnityEngine;
using System.Collections.Generic;

// helper class to locate animals etc in the local area.
public class AnimalMap : MonoBehaviour {
    public static AnimalMap animalMap;

    public List<Animal> animals = new List<Animal>();
    public List<Carrot> carrots = new List<Carrot>();

    private Dictionary<AnimalTypes, List<Herd>> availableHerds;

    void Awake()
    {
        animalMap = this;
        availableHerds = new Dictionary<AnimalTypes, List<Herd>>();
    }

    public float GetFearOfPosition( AnimalTypes type, Vector3 position )
    {
        float maximumFear = 0f;

        for ( int i=0; i < animals.Count; i++ )
        {
            float effectiveFear = AnimalRelationships.Instance.GetOpinionOf(type, animals[i].type).fear;
            effectiveFear = Attenuate(effectiveFear, (animals[i].GetPosition() - position).magnitude);
            if (maximumFear < effectiveFear)
            {
                maximumFear = effectiveFear;
            }
        }

        return maximumFear;
    }

    private float Attenuate( float value, float distance )
    {
        if ( distance > WalkToBehaviour.intimidationRange )
        {
            return 0f;
        } else
        {
            return value * (WalkToBehaviour.intimidationRange - distance) / WalkToBehaviour.intimidationRange;
        }
    }
    
    // returns a herd to the list of available herds.
    public void HerdIsAvailable( Herd herd )
    {
        List<Herd> herdList = null;
        if ( !availableHerds.TryGetValue(herd.type, out herdList) )
        {
            herdList = new List<Herd>();
            availableHerds.Add(herd.type, herdList);
        }

        herdList.Add(herd);
    }

    public void HerdIsFull(Herd herd)
    {
        availableHerds[herd.type].Remove(herd);
    }

    public Herd GetHerdForType(Animal animal)
    {
        List<Herd> herdList = null;
        if ( !availableHerds.TryGetValue(animal.type, out herdList ) )
        {
            // doesn't exist, create a new one.
            herdList = new List<Herd>();
            Herd herd = new Herd();
            herd.type = animal.type;
            herdList.Add(herd);
            availableHerds.Add(animal.type, herdList);
            return herd;
        } else
        {
            if ( herdList.Count == 0 )
            {
                Herd herd = new Herd();
                herd.type = animal.type;
                herdList.Add(herd);
                return herd;
            } else
            {

                float smallestDistance = float.MaxValue;
                Herd closestHerd = null;

                float dist;
                for ( int i=0; i < herdList.Count; i++ )
                {
                    dist = (herdList[i].GetHerdCenter() - animal.GetPosition()).sqrMagnitude;
                    if ( dist < smallestDistance )
                    {
                        smallestDistance = dist;
                        closestHerd = herdList[i];
                    }
                }

                if ( closestHerd == null )
                {
                    Debug.LogError("Wut, no this cant actually happen.");
                }
                return closestHerd;
            }
        }
    }
}
