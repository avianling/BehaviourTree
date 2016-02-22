using UnityEngine;
using System.Collections;

public interface IBehaviour {

    void SetTarget(Animal animal);

    string Description
    {
        get;
    }

    BehaviourState Start();

    BehaviourState Update();

    BehaviourState LastState
    {
        get;
    }

    void End();

}
