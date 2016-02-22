using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Sequencer : CachedTreeInternalNode {

    private int currentSelection;

    public Sequencer(string name) : base(name) { }

    protected override void Reset()
    {
        currentSelection = -1;
    }

    protected override IBehaviour Evaluate()
    {
        currentSelection += 1;
        if ( currentSelection == children.Count)
        {
            return null;
        } else
        {
            return children[currentSelection];
        }
    }


}
