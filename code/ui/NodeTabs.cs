using Godot;
using System;

public class NodeTabs : Tabs
{

    public override void _Ready() {
        AddTab("File");
        AddTab("Text");
        AddTab("Math");
        AddTab("Image");
        AddTab("Other");
    }
}
