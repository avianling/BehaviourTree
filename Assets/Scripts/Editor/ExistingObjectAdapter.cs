using UnityEngine;
using System.Collections;

public class ExistingObjectAdapter : BehaviourTreeAdapter {

    private Animal animal;

    public ExistingObjectAdapter( Animal animal )
    {
        this.animal = animal;
    }

    public TreeNode GetTree()
    {
        TreeNode node = new ExistingTreeNode(animal.behaviour, null);
        node.PositionInAvailableWidth();
        return node;
    }

}
