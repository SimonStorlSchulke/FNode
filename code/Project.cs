using Godot;
using System;

public class Project : Control
{
    public NodeTree NodeTree;
    public TCFilestacks FileStacks;
    public static int idxEval = 0;
    public static int maxNumFiles = 0;

    public override void _Ready() {
        NodeTree = GetNode<NodeTree>("NodeTree");   
        FileStacks = GetNode<TCFilestacks>("TCFilestacks");   
    }
}
