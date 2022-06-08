using Godot;
using System;

public class TCAddNodesPanel : TabContainer
{
    [Export] NodePath NPNodeTree;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CreateAddButton(typeof(FNodeGetFiles), "File");
        CreateAddButton(typeof(FNodeFileInfo), "File");
        CreateAddButton(typeof(FNodeRename), "File");
        CreateAddButton(typeof(FNodeCreateTextFile), "File");
        CreateAddButton(typeof(FNodeJoinStrings), "String");
        CreateAddButton(typeof(FNodeDateToString), "Date");
    }

    void CreateAddButton(Type t, string Category) {
        Button AddNodeButton = new Button();
        AddNodeButton.Text = t.Name.Replace("FNode", "");

        FNode fNode = (FNode)Activator.CreateInstance(t);

        AddNodeButton.Connect("pressed", GetNode(NPNodeTree), "OnAddNode", new Godot.Collections.Array{fNode});
        GetNode<HBoxContainer>(Category).AddChild(AddNodeButton);
    }
}
