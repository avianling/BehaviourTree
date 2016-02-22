using UnityEngine;
using System.Collections.Generic;

public class WanderBehaviour : Behaviour
{
    public const float maxDistance = 3f;

    public WanderBehaviour(string name) : base(name) { }

    protected override BehaviourState RunStart()
    {

        // When picking a position, be repelled by any hated creatures and attracted ( up to a point ) by any liked creatures.
        if (target.herdAnimal)
        {
            Vector2 offset = Random.insideUnitCircle * maxDistance * Random.Range(0.5f, 2.5f);
            target.targetPosition = target.herd.GetHerdCenter() + new Vector3(offset.x, 0, offset.y);
            
        }
        else
        {
            Vector2 offset = Random.insideUnitCircle * maxDistance * Random.Range(0.5f, 2.5f);
            target.targetPosition = target.transform.position + new Vector3(offset.x, 0, offset.y);
        }
        //Debug.DrawLine(target.GetPosition(), target.targetPosition, Color.blue, 5f);
        return BehaviourState.Completed;
    }

    protected override BehaviourState RunUpdate()
    {
        // If we get to this point, we've failed. Should move on automatically on start.
        return BehaviourState.Failed;
    }
}
