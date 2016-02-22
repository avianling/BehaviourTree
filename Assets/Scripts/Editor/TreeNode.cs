using UnityEngine;
using System.Collections.Generic;

//adapter for a node in a tree?
public class TreeNode {
    public List<TreeNode> children = new List<TreeNode>();
    public TreeNode parent;
    public Rect rect;
    public int id;
    public string name;

    public int width, depth;

    // Provides an indicator for what position this is in from the left.
    public float layoutXHint;

    public static int nextID = 0;

    public virtual string GetDescription()
    {
        return "";
    }

    public virtual Color GetHighlighting()
    {
        return Color.black;
    }

    public void PositionInAvailableWidth()
    {
        DetermineLayoutHint(0f, width);
        AutoPosition(width * rect.width);
    }

    private void AutoPosition( float worldWidth )
    {
        rect.y = 100f * depth;
        rect.x = layoutXHint * worldWidth;

        for ( int i=0; i < children.Count; i++ )
        {
            children[i].AutoPosition(worldWidth);
        }
    }

    private float DetermineLayoutHint(float leftMargin, float totalTreeWidth)
    {
        // determines the layout position for this node based on the available space ( determined by the width of this node )
        // and offset it based on the width.
        // Returns the right margin of the available space.
        // Recursively calls this function on it's children.
        layoutXHint = (leftMargin + 0.5f * width) / totalTreeWidth;
        float rightMargin = leftMargin + width;

        float newLeftMargin = leftMargin;
        for (int i = 0; i < children.Count; i++)
        {
            newLeftMargin = children[i].DetermineLayoutHint(newLeftMargin, totalTreeWidth);
        }

        return rightMargin;
    }

    public void CacheTree( Dictionary<int, TreeNode> lookup )
    {
        lookup.Add(id, this);
        for ( int i=0; i < children.Count; i++ )
        {
            children[i].CacheTree(lookup);
        }
    }
}