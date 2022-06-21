using Godot;
using System;

public class TCAddNodesPanel : TabContainer
{
    [Export] NodePath NPNodeTree;

    public void CreateButtons() {
        CreateAddButton<FNodeGetFiles>();
        CreateAddButton<FNodeFileInfo>();
        CreateAddButton<FNodeFileAtIndex>();
        CreateAddButton<FNodeRename>();
        CreateAddButton<FNodeDelete>();
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
        CreateAddButton<FNodeIndexInfo>();
        CreateAddButton<FNodeMath>();
        CreateAddButton<FNodeAccumulateNumber>();
        CreateAddButton<FNodeTextIncludes>();
        CreateAddButton<FNodeReplaceText>();
        CreateAddButton<FNodeSwitch>();
        CreateAddButton<FNodeNumberToText>();
        CreateAddButton<FNodeMove>();
        CreateAddButton<FNodeJoinPaths>();
        CreateAddButton<FNodeCutText>();
        CreateAddButton<FNodeList>();
        CreateAddButton<FNodeGetListItem>();
        CreateAddButton<FNodeRandomNumber>();
        CreateAddButton<FNodeJoinList>();
        CreateAddButton<FNodePickRandom>();
        CreateAddButton<FNodeSplitList>();
        CreateAddButton<FNodeListToText>();
        CreateAddButton<FNodeSplitTextToList>();
        CreateAddButton<FNodeAccumulateList>();
        CreateAddButton<FNodeListComparisons>();
    }

    Resource cursorDragNode;

    public override void _Ready() {
        cursorDragNode =    ResourceLoader.Load("res://theme/icons/cursor_add_node.png");
    }

    void CreateAddButton<fnType>() where fnType : FNode {
        Button AddNodeButton = new Button();
        AddNodeButton.Align = Button.TextAlign.Left;
        FNode fn = (FNode)Activator.CreateInstance(typeof(fnType));
        AddNodeButton.Text = UIUtil.SnakeCaseToWords(typeof(fnType).Name.Replace("FNode", ""));
        AddNodeButton.HintTooltip = fn.HintTooltip;
        //AddNodeButton.Connect("pressed", Main.inst, nameof(Main.OnAddNodeFromUI), new Godot.Collections.Array{fn});
        AddNodeButton.Connect("button_down", this, nameof(StartDrag), new Godot.Collections.Array{fn});
        GetNode(fn.category).AddChild(AddNodeButton);
    }

    FNode draggedFnode = null;
    bool dragging = false;
    public void StartDrag(FNode fn) {
        draggedFnode = fn;
        dragging = true;
        Input.SetCustomMouseCursor(cursorDragNode);
    }

    public override void _Input(InputEvent e) {
        if (dragging == false) {
            return;
        }
        if (e is InputEventMouseButton) {
            if (((InputEventMouseButton)e).ButtonIndex == (int)ButtonList.Left && ((InputEventMouseButton)e).Pressed == false) {
                if (Main.inst.currentProject.NodeTree.MouseOver()) {
                    Main.inst.OnAddNodeFromUI(draggedFnode, false);
                } else {
                    Main.inst.OnAddNodeFromUI(draggedFnode, true);
                }
                dragging = false;
                Input.SetCustomMouseCursor(null);
            }
        }
    }

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("add_viewer")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));
        }
        if(e.IsActionPressed("add_get_files")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeGetFiles)));
        }
        if(e.IsActionPressed("add_fileinfo")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeFileInfo)));
        }
        if(e.IsActionPressed("add_switch")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeSwitch)));
        }
        if(e.IsActionPressed("add_math")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMath)));
        }
        if(e.IsActionPressed("add_move")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMove)));
        }
        if(e.IsActionPressed("add_rename")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeRename)));
        }
        if(e.IsActionPressed("add_del")) {
            /*Main.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));*/
        }
        if(e.IsActionPressed("add_join")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeJoinTexts)));
        }
        if(e.IsActionPressed("add_index")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeIndexInfo)));
        }
        if(e.IsActionPressed("add_accumulate_string")) {
            Main.inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeAccumulateText)));
        }

        if(e.IsActionPressed("evaluate_tree")) { //TODO put this somewhere else
            Main.inst.OnParseTree(true);
        }
    }
}
