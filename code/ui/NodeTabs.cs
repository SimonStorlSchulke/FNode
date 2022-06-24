using Godot;
using System;

public class NodeTabs : Tabs
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddTab("File");
        AddTab("Text");
        AddTab("Math");
        AddTab("Image");
        AddTab("Other");
    }

}
