using Godot;
using System;

public class TCAddNodesPanel : TabContainer
{
    [Export] NodePath NPNodeTree;

    public override void _Ready()
    {
        CreateAddButton(typeof(FNodeGetFiles), "File");
        CreateAddButton(typeof(FNodeFileInfo), "File");
        CreateAddButton(typeof(FNodeRename), "File");
        CreateAddButton(typeof(FNodeFilterFiles), "File");
        CreateAddButton(typeof(FNodeCreateTextFile), "File");
        CreateAddButton(typeof(FNodeJoinStrings), "String");
        CreateAddButton(typeof(FNodeTextViewer), "String");
        CreateAddButton(typeof(FNodeDateToString), "Date");
        CreateAddButton(typeof(FNodeDateCompare), "Date");
        CreateAddButton(typeof(FNodeCurrentDate), "Date");
        CreateAddButton(typeof(FNodeGetParentPath), "File");
    }

    void CreateAddButton(Type t, string Category) {
        Button AddNodeButton = new Button();
        AddNodeButton.Text = t.Name.Replace("FNode", "");

        FNode fNode = (FNode)Activator.CreateInstance(t);

        AddNodeButton.Connect("pressed", GetNode(NPNodeTree), "OnAddNode", new Godot.Collections.Array{fNode});
        GetNode<HBoxContainer>(Category).AddChild(AddNodeButton);
    }
}
