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
        CreateAddButton<FNodeJoinTexts>();
        CreateAddButton<FNodeAccumulateText>();
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
        CreateAddButton<FNodeTextIncludes>();
        CreateAddButton<FNodeReplaceText>();
        CreateAddButton<FNodeSwitch>();
        CreateAddButton<FNodeNumberToText>();
        CreateAddButton<FNodeMove>();
        CreateAddButton<FNodeJoinPaths>();
    }

    void CreateAddButton<fnType>() where fnType : FNode {
        Button AddNodeButton = new Button();
        AddNodeButton.Align = Button.TextAlign.Left;
        FNode fn = (FNode)Activator.CreateInstance(typeof(fnType));
        AddNodeButton.Text = UIUtil.SnakeCaseToWords(typeof(fnType).Name.Replace("FNode", ""));
        AddNodeButton.Connect("pressed", GetNode(NPNodeTree), nameof(NodeTree.OnAddNodeFromUI), new Godot.Collections.Array{fn});
        GetNode(fn.category).AddChild(AddNodeButton);
    }

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("add_viewer")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));
        }
        if(e.IsActionPressed("add_get_files")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeGetFilesFromStack)));
        }
        if(e.IsActionPressed("add_fileinfo")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeFileInfo)));
        }
        if(e.IsActionPressed("add_switch")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeSwitch)));
        }
        if(e.IsActionPressed("add_math")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMath)));
        }
        if(e.IsActionPressed("add_move")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMove)));
        }
        if(e.IsActionPressed("add_rename")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeRename)));
        }
        if(e.IsActionPressed("add_del")) {
            /*Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));*/
        }
        if(e.IsActionPressed("add_join")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeJoinTexts)));
        }
        if(e.IsActionPressed("add_index")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeIndex)));
        }
        if(e.IsActionPressed("add_accumulate_string")) {
            Main.inst.currentProject.NodeTree.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeAccumulateText)));
        }

        if(e.IsActionPressed("evaluate_tree")) { //TODO put this somewhere else
            Main.inst.currentProject.NodeTree.EvaluateTree();
        }
    }
}
