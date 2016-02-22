using UnityEngine;
using System.Collections.Generic;

public class AnimalFactory : MonoBehaviour {

    public Animal foxPrefab;
    public Animal bunnyPrefab;
    public Animal shepardPrefab;
    public Carrot carrotPrefab;

    public static AnimalFactory Instance;

    void Awake()
    {
        Instance = this;
    }

    public Animal CreateAnimal( AnimalTypes type )
    {
        Animal prefab = null;
        IBehaviour behaviour = null;
        switch (type)
        {
            case AnimalTypes.Bunny:
                prefab = bunnyPrefab;

                // Create the new bunny AI.
                behaviour = CreateBunnyAI();
                break;
            case AnimalTypes.Fox:
                prefab = foxPrefab;
                behaviour = CreateBunnyAI();

                break;
            case AnimalTypes.Shepard:
                prefab = shepardPrefab;
                behaviour = CreateBunnyAI();
                break;
        }

        Animal newObj = GameObject.Instantiate<Animal>(prefab);
        newObj.behaviour = behaviour;
        AnimalMap.animalMap.animals.Add(newObj);
        return newObj;
    }


    private IBehaviour CreateBunnyAI()
    {
        Selector mainSelector = new Selector("Main Selector");

        // Idle subtree.
        Selector idleSelector = new Selector("Idle Selector");

        // Idle -> Wander tree
        TreeInternalNode wanderSequencer = new Sequencer("Wander Behaviour");
        IBehaviour findWanderTarget = new WanderBehaviour("Find Wander Target");
        IBehaviour moveToPosition = new WalkToBehaviour("Walk to target");
        wanderSequencer.children.Add(findWanderTarget);
        wanderSequencer.children.Add(moveToPosition);
        Repeater wanderRepeater = new Repeater();
        wanderRepeater.child = wanderSequencer;

        AnonUtilityWrapper wanderDesire = new AnonUtilityWrapper();
        wanderDesire.child = wanderSequencer;
        wanderDesire.utilityFunction = (x) => { return 0.5f; };

        // Idle -> Breed Tree.
        IBehaviour findMateBehaviour = new PickSameSpecies("Find a mate");
        IBehaviour walkToMateBehaviour = new WalkToTarget("Walk to the mate");
        IBehaviour breedBehaviour = new Breed("");
        Sequencer breedSequencer = new Sequencer("Breeding");
        breedSequencer.children.AddRange(new IBehaviour[] { findMateBehaviour, walkToMateBehaviour, breedBehaviour });

        AnonUtilityWrapper breedDesire = new AnonUtilityWrapper();
        breedDesire.child = breedSequencer;
        breedDesire.utilityFunction = (x) =>
        {
            return x.target.BreedDesire();
        };

        idleSelector.children.AddRange(new IBehaviour[] { wanderDesire, breedDesire });

        AnonUtilityWrapper idleDesire = new AnonUtilityWrapper();
        idleDesire.child = idleSelector;
        idleDesire.utilityFunction = (x) => { return 0.5f; };

        // Find and eat food subtree.
        TreeInternalNode findFoodSubtree = new Sequencer("Find Food Subtree");
        IBehaviour findFood = new FindFood("Find Food");
        IBehaviour walkToFood = new WalkToTarget("Walk To Food");
        IBehaviour eat = new Eat("Eat Food");
        findFoodSubtree.children.AddRange( new IBehaviour[]{findFood, walkToFood, eat });
        AnonUtilityWrapper eatFoodDesire = new AnonUtilityWrapper("Desire for food.");
        eatFoodDesire.child = findFoodSubtree;
        eatFoodDesire.utilityFunction = (x) =>
        {
            if (x.target.eating)
            {
                return 1f;
            }
            else
            {
                return (x.target.hunger - 15f) / 15f;
            }
        };

        // Flee Danger Subtree
        Repeater avoidTreeRepeater = new Repeater();
        Sequencer avoidDangerTree = new Sequencer("Running Away Subtree");
        avoidTreeRepeater.child = avoidDangerTree;
        IBehaviour avoidDanger = new AvoidDanger("Work out where to run");
        IBehaviour walkAway = new WalkToBehaviour("Do the running away");
        avoidDangerTree.children.AddRange(new IBehaviour[] { avoidDanger, walkAway });
        AnonUtilityWrapper avoidDangerDesire = new AnonUtilityWrapper("Current danger estimate");
        avoidDangerDesire.SetChild(avoidTreeRepeater);
        avoidDangerDesire.utilityFunction = (x) =>
        {
            if ( x.target.predator )
            {
                return 0f;
            }
            if ( x.target.hunter == null )
            {
                return 0f;
            } else
            {
                float distance = (x.target.hunter.transform.position - x.target.transform.position).magnitude;
                if ( distance < 8f )
                {
                    return (8f - distance) / 3f;
                } else
                {
                    return 0f;
                }
            }
        };

        // Attack Hated behaviours.
        FindHatedTarget findTarget = new FindHatedTarget("");
        WalkToTarget moveToTarget = new WalkToTarget("Approach victim");
        Attack attackTarget = new Attack();
        Sequencer attackHatedSequencer = new Sequencer("Attack Hated Creatures");
        attackHatedSequencer.children.AddRange(new IBehaviour[] { findTarget, moveToTarget, attackTarget });

        AnonUtilityWrapper attackHatedDecider = new AnonUtilityWrapper("Impact of nearby hated units");
        attackHatedDecider.SetChild(attackHatedSequencer);
        attackHatedDecider.utilityFunction = (x) => { return findTarget.GetUtility(); };

        mainSelector.children.AddRange(new IBehaviour[] { idleDesire, eatFoodDesire, avoidDangerDesire, attackHatedDecider});

        Repeater repeatAll = new Repeater();
        repeatAll.SetChild(mainSelector);
        return repeatAll;
    }

    public Carrot CreateCarrot()
    {
        Carrot carrot = GameObject.Instantiate<Carrot>(carrotPrefab);
        carrot.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        AnimalMap.animalMap.carrots.Add(carrot);
        return carrot;
    }

}
