using Godot;


public class UIInput : Node
{
    [Export] NodePath NPHelpPanel;

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_delete")) {
            TCFilestacks fileStacks = Main.inst.currentProject.FileStacks;
            int selectedStack = Main.inst.currentProject.FileStacks.CurrentTab;
            fileStacks.GetCurrentFileList().RemoveSelectedItems();
        } else if (e.IsActionPressed("ui_help")) {
            GetNode<Popup>(NPHelpPanel).PopupCentered();
        }
    }
}
