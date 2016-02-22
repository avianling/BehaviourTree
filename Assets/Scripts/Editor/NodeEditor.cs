using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeEditor : EditorWindow
{
    public TreeNode node;
    public BehaviourTreeAdapter adapter;
    private Object cachedTarget;
    public Dictionary<int, TreeNode> treeNodeLookup = new Dictionary<int, TreeNode>();

    [MenuItem("Window/Node editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        editor.Init();
    }

    public void Init()
    {
        GUI.skin.window.normal.textColor = Color.white;
        Init(Selection.activeObject);
    }

    public void Init( Object obj)
    {
        cachedTarget = obj;
        treeNodeLookup.Clear();
        node = null;

        if (obj is GameObject)
        {
            Animal animal = (obj as GameObject).GetComponent<Animal>();
            if ( animal == null )
            {
                return;
            }
            adapter = new ExistingObjectAdapter(animal);
            node = adapter.GetTree();
        }

        // Cache the tree!
        if (node != null)
        {
            node.CacheTree(treeNodeLookup);
        }
    }

    void Update()
    {
        Repaint();

        if ( Selection.activeObject != cachedTarget )
        {
            Init(Selection.activeObject);
        }
    }

    void OnGUI()
    {
        BeginWindows();
        DrawWithChildren(node);
        GUI.skin.window.normal.textColor = Color.white;
        EndWindows();
    }

    void DrawWithChildren( TreeNode node )
    {
        if (node != null)
        {
            node.rect = GUI.Window(node.id, node.rect, DrawNodeWindow, node.name);

            for (int i = 0; i < node.children.Count; i++)
            {
                DrawNodeCurve(node, node.children[i]);
                DrawWithChildren(node.children[i]);
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        GUILayout.Label(treeNodeLookup[id].GetDescription());
        GUI.DragWindow();
    }

    void DrawNodeCurve(TreeNode origin, TreeNode target)
    {
        Rect start = origin.rect;
        Rect end = target.rect;
        Vector3 startPos = new Vector3(start.x + start.width * 0.5f, start.y + end.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width * 0.5f, end.y, 0);
        Vector3 startTan = startPos + Vector3.up * 50;
        Vector3 endTan = endPos + Vector3.down * 50;

        Color lineColor = target.GetHighlighting();

        Color shadowCol = lineColor;
        shadowCol.a = 0.06f;
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);

        Handles.DrawBezier(startPos, endPos, startTan, endTan, lineColor, null, 1);
    }
}