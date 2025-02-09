using System.Collections.Generic;
using Godot;
using System;

public partial class TCAddNodesPanel : Control
{
    [Export] NodePath NPSearchResults;
    VBSearchResults searchResults;

    [Export] Godot.Collections.Dictionary<string, Color> buttonColors;
    Godot.Collections.Dictionary<string, StyleBoxFlat> buttonColorStyles = new Godot.Collections.Dictionary<string, StyleBoxFlat>();
    //Godot.Collections.Dictionary<string, StyleBoxFlat> buttonColorStyles = new Godot.Collections.Dictionary<string, StyleBoxFlat>();

    public void CreateButtons() {
        /*Could be automated by looping over classes that extend FNode...
        but for now keep the choice of not making Nodes aviable in the UI*/
        CreateAddButton<FNodeGetFiles>();
        CreateAddButton<FNodeIntNumber>();
        CreateAddButton<FNodeFloatNumber>();
        CreateAddButton<FNodeFileInfo>();
        CreateAddButton<FNodeFileAtIndex>();
        CreateAddButton<FNodeFileExists>();
        CreateAddButton<FNodeRename>();
        CreateAddButton<FNodeDelete>();
        CreateAddButton<FNodeFilterFiles>();
        CreateAddButton<FNodeCreateTextFile>();
        CreateAddButton<FNodeText>();
        CreateAddButton<FNodeJoinTexts>();
        CreateAddButton<FNodeTextViewer>();
        CreateAddButton<FNodeDateToString>();
        CreateAddButton<FNodeDateCompare>();
        CreateAddButton<FNodeCurrentDate>();
        CreateAddButton<FNodeGetParentPath>();
        CreateAddButton<FNodeIndexInfo>();
        CreateAddButton<FNodeMath>();
        CreateAddButton<FNodeAccumulateNumber>();
        CreateAddButton<FNodeTextIncludes>();
        CreateAddButton<FNodeReplaceText>();
        CreateAddButton<FNodeValueSwitch>();
        CreateAddButton<FNodeNumberToText>();
        CreateAddButton<FNodeMoveFile>();
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
        CreateAddButton<FNodeGetImage>();
        CreateAddButton<FNodeMergeImages>();
        CreateAddButton<FNodeFilterImages>();
        CreateAddButton<FNodeSaveImageAs>();
        CreateAddButton<FNodeImageViewer>();
        CreateAddButton<FNodeResizeImage>();
        CreateAddButton<FNodeRotateImage>();
        CreateAddButton<FNodeImageInfo>();
        CreateAddButton<FNodeTrimImage>();
        CreateAddButton<FNodeHttpRequest>();
        CreateAddButton<FNodeGetJSONItem>();
        CreateAddButton<FNodeAddBackground>();
        CreateAddButton<FNodeCropImage>();
    }

    Resource cursorDragNode;

    public override void _Ready() {
        cursorDragNode =    ResourceLoader.Load("res://theme/icons/cursor_add_node.png");
        searchResults = GetNode<VBSearchResults>(NPSearchResults);

        // Create Styleboxes for Button Categories
        foreach (var col in buttonColors) {
            var sb = new StyleBoxFlat();
            sb.BorderWidthLeft = 12;
            sb.ContentMarginLeft = 33;
            sb.BorderColor = col.Value;
            sb.BgColor = new Color("#363e42");
            buttonColorStyles.Add(col.Key, sb);
        }

        foreach (CheckButton tab in GetNode("VBTabs").GetChildren()) {
            tab.Pressed += () => OnChangeTab(tab.GetIndex());
        }
    }

    void OnChangeTab(int TabIndex) {
        int i = 0;
        foreach (CheckButton tab in GetNode("VBTabs").GetChildren()) {
            if (TabIndex != i) {
                tab.SetPressed(false);;
            }
            i++;
        }

        i = 0;
        foreach (Control tabPanel in GetNode("Panels").GetChildren()) {
            if (TabIndex == i) {
                tabPanel.Visible = true;
            } else {
                tabPanel.Visible = false;
            }
            i++;
        }
    }


    void CreateAddButton<fnType>() where fnType : FNode {
        Button AddNodeButton = new Button();
        AddNodeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddNodeButton.SetTextAlignment(HorizontalAlignment.Left);
        FNode fn = (FNode)Activator.CreateInstance(typeof(fnType));
        AddNodeButton.Text = UIUtil.SnakeCaseToWords(typeof(fnType).Name.Replace("FNode", ""));
        AddNodeButton.TooltipText = fn.TooltipText;
        AddNodeButton.ButtonDown += () => StartDrag(fn);

        AddNodeButton.CustomMinimumSize = new Vector2(0, 50);
        AddNodeButton.AddThemeStyleboxOverride("normal", buttonColorStyles[fn.category]);
        AddNodeButton.AddThemeStyleboxOverride("hover", buttonColorStyles[fn.category]);
        AddNodeButton.AddThemeStyleboxOverride("pressed", buttonColorStyles[fn.category]);
        AddNodeButton.AddThemeStyleboxOverride("focus", buttonColorStyles[fn.category]);

        GetNode("Panels/" + fn.category).AddChild(AddNodeButton);
        Button searchResultButton = (Button)AddNodeButton.Duplicate();
        searchResultButton.ButtonDown += () => StartDrag(fn);
        searchResultButton.Visible = false;
        searchResults.AddChild(searchResultButton);
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
            if (((InputEventMouseButton)e).ButtonIndex == MouseButton.Left && ((InputEventMouseButton)e).IsPressed() == false) {
                if (Main.Inst.CurrentProject.NodeTree.MouseOver()) {
                    Main.Inst.OnAddNodeFromUI(draggedFnode, false);
                } else {
                    Main.Inst.OnAddNodeFromUI(draggedFnode, true);
                }
                dragging = false;
                Input.SetCustomMouseCursor(null);
            }
        }
    }

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("add_viewer")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));
        }
        if(e.IsActionPressed("add_get_files")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeGetFiles)));
        }
        if(e.IsActionPressed("add_fileinfo")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeFileInfo)));
        }
        if(e.IsActionPressed("add_switch")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeSwitch)));
        }
        if(e.IsActionPressed("add_math")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMath)));
        }
        if(e.IsActionPressed("add_move")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeMoveFile)));
        }
        if(e.IsActionPressed("add_rename")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeRename)));
        }
        if(e.IsActionPressed("add_del")) {
            /*Main.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeTextViewer)));*/
        }
        if(e.IsActionPressed("add_join")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeJoinTexts)));
        }
        if(e.IsActionPressed("add_index")) {
            Main.Inst.OnAddNodeFromUI(
                (FNode)Activator.CreateInstance(typeof(FNodeIndexInfo)));
        }
        if(e.IsActionPressed("evaluate_tree_preview")) { //TODO put this somewhere else
            Main.Inst.OnParseTree(true);
        }
        if(e.IsActionPressed("evaluate_tree")) { //TODO put this somewhere else
            Main.Inst.OnParseTree(false);
        }
    }
}
