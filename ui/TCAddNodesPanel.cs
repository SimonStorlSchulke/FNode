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
        CreateAddButton(typeof(FNodeGetExifData), "File");
        CreateAddButton(typeof(FNodeCreateTextFile), "File");
        CreateAddButton(typeof(FNodeJoinStrings), "String");
        CreateAddButton(typeof(FNodeAccumulateString), "String");
        CreateAddButton(typeof(FNodeTextViewer), "String");
        CreateAddButton(typeof(FNodeCopy), "File");
        CreateAddButton(typeof(FNodeJoinPaths), "String");
        CreateAddButton(typeof(FNodeTextField), "String");
        CreateAddButton(typeof(FNodeDateToString), "Date");
        CreateAddButton(typeof(FNodeDateCompare), "Date");
        CreateAddButton(typeof(FNodeCurrentDate), "Date");
        CreateAddButton(typeof(FNodeGetParentPath), "File");
        CreateAddButton(typeof(FNodeGetFilesFromStack), "File");
        CreateAddButton(typeof(FNodeIndex), "Math");
        CreateAddButton(typeof(FNodeMath), "Math");
        CreateAddButton(typeof(FNodeAccumulateNumber), "Math");
        CreateAddButton(typeof(FNodeStringIncludes), "String");
        CreateAddButton(typeof(FNodeReplaceString), "String");
        CreateAddButton(typeof(FNodeSwitch), "Other");
    }

    void CreateAddButton(Type t, string Category) {
        Button AddNodeButton = new Button();
        AddNodeButton.Text = t.Name.Replace("FNode", "");
        AddNodeButton.Align = Button.TextAlign.Left;

        FNode fNode = (FNode)Activator.CreateInstance(t);

        AddNodeButton.Connect("pressed", GetNode(NPNodeTree), "OnAddNode", new Godot.Collections.Array{fNode});
        GetNode<Control>(Category).AddChild(AddNodeButton);
    }

    public override void _UnhandledInput(InputEvent e)
    {
    if (e.IsActionPressed("nd_switch"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeSwitch());
    else if (e.IsActionPressed("nd_viewer"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeTextViewer());
    else if (e.IsActionPressed("nd_math"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeMath());
    else if (e.IsActionPressed("nd_get_files"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeGetFilesFromStack());
    else if (e.IsActionPressed("nd_file"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeGetFiles());
    else if (e.IsActionPressed("nd_file_info"))
        GetNode<NodeTree>(NPNodeTree).OnAddNode(new FNodeFileInfo());
}
}
