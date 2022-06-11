using Godot;
using System;

public class TCAddNodesPanel : TabContainer
{
    [Export] NodePath NPNodeTree;

    public override void _Ready()
    {
        CreateAddButton<FNodeGetFiles>();
        CreateAddButton<FNodeFileInfo>();
        CreateAddButton<FNodeRename>();
        CreateAddButton<FNodeFilterFiles>();
        CreateAddButton<FNodeCreateTextFile>();
        CreateAddButton<FNodeJoinStrings>();
        CreateAddButton<FNodeAccumulateString>();
        CreateAddButton<FNodeTextViewer>();
        CreateAddButton<FNodeTextField>();
        CreateAddButton<FNodeDateToString>();
        CreateAddButton<FNodeDateCompare>();
        CreateAddButton<FNodeCurrentDate>();
        CreateAddButton<FNodeGetParentPath>();
        CreateAddButton<FNodeGetFilesFromStack>();
        CreateAddButton<FNodeIndex>();
        CreateAddButton<FNodeMath>();
        CreateAddButton<FNodeAccumulateNumber>();
        CreateAddButton<FNodeStringIncludes>();
        CreateAddButton<FNodeReplaceString>();
        CreateAddButton<FNodeSwitch>();
    }

    void CreateAddButton<fnType>() where fnType : FNode {
        Button AddNodeButton = new Button();
        AddNodeButton.Align = Button.TextAlign.Left;
        FNode fn = (FNode)Activator.CreateInstance(typeof(fnType));
        AddNodeButton.Text = typeof(fnType).Name.Replace("FNode", "");
        AddNodeButton.Connect("pressed", GetNode(NPNodeTree), nameof(NodeTree.OnAddNodeFromUI), new Godot.Collections.Array{fn});
        GetNode(fn.category).AddChild(AddNodeButton);
    }

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsAction("add_viewer")) {
            //Main.inst.currentProject.NodeTree.OnAddNodeFromUI<))
        }
    }
}
