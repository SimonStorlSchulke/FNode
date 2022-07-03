using Godot;
using System;

public class VBSearchResults : VBoxContainer
{
    Resource cursorDragNode;

    public override void _Ready() {
        cursorDragNode = ResourceLoader.Load("res://theme/icons/cursor_add_node.png");
        GetParent().GetParent().GetChild<LineEdit>(0).FocusMode = FocusModeEnum.All;
    }

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_search")) {
            GetParent().GetParent().GetChild<LineEdit>(0).GrabFocus();
        }
    }


    public void OnSearchNode(string str) {
        var childs = GetChildren();
        bool hadResults = false;
        foreach (Button item in childs) {
            if (DevUtil.StringContains(item.Text, str, StringComparison.OrdinalIgnoreCase) && str != "") {
                item.Visible = true;
                hadResults = true;
            } else {
                item.Visible = false;
            }
        }

        GetParent<ScrollContainer>().Visible = hadResults;
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
}
