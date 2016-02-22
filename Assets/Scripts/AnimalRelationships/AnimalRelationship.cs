using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimalRelationship {
    public AnimalTypes source, target;

    // How afraid of this creature are we?
    // increases when they kill one of us.
    // causes animals to run away & avoid conflict.
    public float fear;

    // How much do we hate this creature?
    // Determines how much we will try to attack them.
    // Increases by them taking our stuff.
    public float hate;

    // Increased by the target creature helping us, giving gifts.
    // Opposes both fear and hate.
    public float love;
}
