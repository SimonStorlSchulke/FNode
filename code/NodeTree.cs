using Godot;
using System.Collections.Generic;
using System;

public class NodeTree : GraphEdit
{

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public List<FNode> GetFNodes() {

        List<FNode> fNodes = new List<FNode>();

        foreach (var node in GetChildren()) {
            if (node is FNode) {
                fNodes.Add(node as FNode);
            }
        }

        return fNodes;
    }

    public void TestTree(string root) {
        FNode n = GetNode<FNode>(root);
        EvaluateTree(n);
    }

    public void EvaluateTree(FNode Root) {

        foreach (Node fn in GetChildren()) {
            if (fn is FNode) {
                if ((fn as FNode).isExecutiveNode) {
                    (fn as FNode).ExecutiveMethod();
                }
            }
        }
        object v = Root.outputs["String"].Get();
        GD.Print(v);
    }

    public void OnConnectionRequest(string from, int fromSlot, string to, int toSlot) {
        ConnectNode(from, fromSlot, to, toSlot);
    }

    public void OnDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
    }

    public void OnAddNode(FNode fn) {
        AddChild(fn.Duplicate());
    }
}
