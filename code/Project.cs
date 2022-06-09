using Godot;
using System;

public class Project : Control
{
    public NodeTree NodeTree;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        NodeTree = GetNode<NodeTree>("NodeTree");   
    }

}
