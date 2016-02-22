using UnityEngine;
using System.Collections;

public class Selector : TreeInternalNode {

    public Selector(string name) : base(name)
    {

    }

    protected override IBehaviour Evaluate()
    {
        if ( children.Count == 0 )
        {
            return null;
        } else
        {
            IBehaviour bestBehaviour = children[0];
            float bestScore = 0;

            for ( int i=0; i < children.Count; i++ )
            {
                IUtilityBehaviour utilityBehaviour = children[i] as IUtilityBehaviour;
                if ( utilityBehaviour != null )
                {
                    float newScore = utilityBehaviour.Utility();
                    if ( newScore > bestScore )
                    {
                        bestScore = newScore;
                        bestBehaviour = utilityBehaviour;
                    }
                }
            }

            return bestBehaviour;
        }
    }

}
