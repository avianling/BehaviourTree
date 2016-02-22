using UnityEngine;
using System.Collections;
using System;

public class Carrot : MonoBehaviour, ITargettable {

    public float foodRemaining;
    public Animal beingEatenBy;

    public void Targetted(Animal animal)
    {
        beingEatenBy = animal;
    }

    public void Untargetted(Animal animal)
    {
        if ( beingEatenBy == animal )
        {
            beingEatenBy = null;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    void Start()
    {
        Update();
    }

    void Update()
    {
        if ( foodRemaining < 10f )
        {
            foodRemaining += 2f * Time.deltaTime;
        }

        transform.localScale = Vector3.one * (foodRemaining / 10f);
    }

    public void Die()
    {
        AnimalMap.animalMap.carrots.Remove(this);
        if ( beingEatenBy != null )
        {
            beingEatenBy.target = null;
        }
        Destroy(gameObject);
    }

    public bool CanBeTargetted(Animal animal)
    {
        return beingEatenBy == null || beingEatenBy == animal;
    }

    public bool IsAlive()
    {
        return this != null;
    }
}
