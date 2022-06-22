using Godot;
using System;

public class VBSearchResults : VBoxContainer
{
    Resource cursorDragNode;

    public override void _Ready() {
        cursorDragNode = ResourceLoader.Load("res://theme/icons/cursor_add_node.png");
    }

    public void OnSearchNode(string str) {
        var childs = GetChildren();
        foreach (Button item in childs) {
            if (item.Text.Contains(str) && str != "") {
                item.Visible = true;
            } else {
                item.Visible = false;
            }
        }
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
}
