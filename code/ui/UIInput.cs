using Godot;

public partial class UIInput : Node
{
    [Export] NodePath NPHelpPanel;
    [Export] NodePath NPIO;

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_delete")) {
            TCFilestacks fileStacks = Main.Inst.CurrentProject.FileStacks;
            int selectedStack = Main.Inst.CurrentProject.FileStacks.CurrentTab;
            fileStacks.GetCurrentFileList().RemoveSelectedItems();
        } 
        else if (e.IsActionPressed("ui_help")) {
            GetNode<Popup>(NPHelpPanel).PopupCentered();
        } 
        else if (e.IsActionPressed("ui_save")) {
            GetNode<IO>(NPIO).OnPopupSave();
        } 
        else if (e.IsActionPressed("ui_load")) {
            GetNode<IO>(NPIO).OnPopupLoad();
        } 
        else if (e.IsActionPressed("ui_copy")) {
            IO.CopySelectedNodes(Main.Inst.CurrentProject.NodeTree);
        } 
        else if (e.IsActionPressed("ui_paste")) {
            IO.PasteNodes();
        } 
        else if (e.IsActionPressed("background_black")) {
            StyleBoxFlat sbBlack = new StyleBoxFlat();
            sbBlack.BgColor = Colors.Black;
            if (Main.Inst.CurrentProject.NodeTree.HasThemeStyleboxOverride("bg")) {
                Main.Inst.CurrentProject.NodeTree.AddThemeStyleboxOverride("bg", null);
            } else {
                Main.Inst.CurrentProject.NodeTree.AddThemeStyleboxOverride("bg", sbBlack);
            }
        }
    }
}
