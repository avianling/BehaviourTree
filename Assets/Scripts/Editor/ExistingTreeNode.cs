using UnityEngine;
using System.Collections;

/// <summary>
/// Defines a tree node in the context of an existing animal.
/// </summary>
public class ExistingTreeNode : TreeNode {

    // should these be tied to IBehaviour or to BehaviourDefinitions?
    // should be able to deal with both, and to handle editing?
    public IBehaviour underlyingBehaviour;

    public ExistingTreeNode(IBehaviour behaviour, TreeNode root)
    {
        parent = root;
        if ( root == null )
        {
            depth = 0;
        } else
        {
            depth = parent.depth + 1;
        }
        rect = new Rect(0, 0, 150, 50);
        id = nextID;
        nextID++;

        name = behaviour.GetType().Name;

        underlyingBehaviour = behaviour;
        if (behaviour is TreeInternalNode)
        {
            TreeInternalNode childNode = behaviour as TreeInternalNode;
            for (int i = 0; i < childNode.children.Count; i++)
            {
                children.Add(new ExistingTreeNode(childNode.children[i], this));
            }
        }
        if (behaviour is Decorator)
        {
            Decorator decorator = behaviour as Decorator;
            children.Add(new ExistingTreeNode(decorator.child, this));
        }

        width = 1;
        int totalChildWidth = 0;
        for (int i = 0; i < children.Count; i++)
        {
            totalChildWidth += children[i].width;
        }
        if ( totalChildWidth > 1)
        {
            width = totalChildWidth;
        }
    }

    public override string GetDescription()
    {
        return underlyingBehaviour.Description;
    }

    public override Color GetHighlighting()
    {
        if ( underlyingBehaviour.LastState == BehaviourState.Running )
        {
            return Color.yellow;
        } else
        {
            if ( underlyingBehaviour.LastState == BehaviourState.Failed )
            {
                return Color.red;
            }
            return Color.black;
        }
    }
}
