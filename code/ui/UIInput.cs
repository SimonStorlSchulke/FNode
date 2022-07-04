using Godot;


public class UIInput : Node
{
    [Export] NodePath NPHelpPanel;
    [Export] NodePath NPIO;

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_delete")) {
            TCFilestacks fileStacks = Main.Inst.CurrentProject.FileStacks;
            int selectedStack = Main.Inst.CurrentProject.FileStacks.CurrentTab;
            fileStacks.GetCurrentFileList().RemoveSelectedItems();
        } else if (e.IsActionPressed("ui_help")) {
            GetNode<Popup>(NPHelpPanel).PopupCentered();
        } else if (e.IsActionPressed("ui_save")) {
            GetNode<IO>(NPIO).OnPopupSave();
        } else if (e.IsActionPressed("ui_load")) {
            GetNode<IO>(NPIO).OnPopupLoad();
        }
    }
}
