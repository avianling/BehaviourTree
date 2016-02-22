using UnityEngine;

public class Animal : MonoBehaviour, ITargettable {

    public IBehaviour behaviour;

    public float hunger;
    public int timesEaten;
    public int breedThreshold = 3;
    public bool eating = false;
    public float age;

    public bool herdAnimal;
    public bool loneWolf;

    public Herd herd;

    public Animal hunter;
    public Animal friendlyTargetter;

    [Range(2f,4f)]
    public float speed;
    public AnimalTypes type;
    public float hungerModifier = 1f;

    public bool predator = false;

    public Vector3 targetPosition;
    private ITargettable actualTarget;
    public ITargettable target
    {
        get
        {
            return actualTarget;
        }
        set
        {
            if (actualTarget != value)
            {
                if (actualTarget != null)
                {
                    actualTarget.Untargetted(this);
                }
                if (value != null)
                {
                    value.Targetted(this);
                }
                actualTarget = value;
            }
        }
    }

    protected virtual IBehaviour BuildTree()
    {
        Sequencer root = new Sequencer("test");
        WalkToBehaviour walkToOne = new WalkToBehaviour("Walk 1");

        WalkToBehaviour walkToTwo = new WalkToBehaviour("Walk 2");

        WalkToBehaviour walkToThree = new WalkToBehaviour("Walk 3");

        root.children.Add(walkToOne);
        root.children.Add(walkToTwo);
        root.children.Add(walkToThree);

        Repeater repeater = new Repeater();
        repeater.child = root;
        return repeater;
    }
    
    void Start()
    {
        if ( herdAnimal )
        {
            herd = AnimalMap.animalMap.GetHerdForType(this);
            herd.AddMember(this);
        }

        hunger = Random.Range(0f, 5f);
        if (behaviour == null)
        {
            behaviour = BuildTree();
        }
        behaviour.SetTarget(this);
        age = 0;
        behaviour.Start();
    }

    void Update()
    {
        if ( behaviour.Update() == BehaviourState.Failed )
        {
            Debug.LogError("BehaviourFailure", this);
        }

        if ( age < 1f )
        {
            age += Time.deltaTime * 0.5f;
        }
        hunger += Time.deltaTime * hungerModifier;
        if ( hunger < 0.5f )
        {
            eating = false;
        }

        if (hunger > 40 )
        {
            Die();
        }
    }

    public void Targetted(Animal animal)
    {
        if (animal.predator)
        {
            hunter = animal;
        } else
        {
            friendlyTargetter = animal;
        }
    }

    public void Untargetted(Animal animal)
    {
        if (animal.predator)
        {
            if (hunter == animal)
            {
                hunter = null;
            }
        } else
        {
            if ( friendlyTargetter == animal )
            {
                friendlyTargetter = null;
            }
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    protected void Die()
    {
        AnimalMap.animalMap.animals.Remove(this);
        if ( herd != null )
        {
            herd.RemoveMember(this);
        }
        target = null;
        Destroy(gameObject);
    }

    public void Killed(Animal murderer)
    {
        AnimalRelationships.Instance.AnimalWasKilled(murderer, this);
        Die();
    }

    public bool CanBeTargetted(Animal animal)
    {
        return (hunter == null || hunter == animal) && animal != this;
    }

    public float BreedDesire()
    {
        // can breed if we are well fed.
        if (timesEaten > breedThreshold && age >= 1f)
        {
            return 1f;
        } else
        {
            return 0f;
        }
    }

    // How much are we willing to put ourself into danger to eat?
    public float HungerDesperation()
    {
        if ( hunger < 20 )
        {
            return 0f;
        } else
        {
            if ( hunger > 40 )
            {
                return 1f;
            }
            return (hunger - 20f) / 20f;
        }
    }

    public bool IsAlive()
    {
        return this != null;
    }
}
