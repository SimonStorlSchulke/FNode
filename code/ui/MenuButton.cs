using Godot;

public class MenuButton : Godot.MenuButton
{
    [Export] NodePath NPIO;
    [Export] NodePath NPPUHotkey;
    IO io;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        io = GetNode<IO>(NPIO);
        GetPopup().Connect("id_pressed", this, nameof(OnItemPressed));
    }

    public void OnItemPressed(int idx) {
        string itemText = GetPopup().GetItemText(idx);
        switch (itemText) {
            case "Save":
                io.OnPopupSave();
                break;
            case "Load":
                io.OnPopupLoad();
                break;
            case "Close Project":
                Main.inst.CloseProject();
                break;
            case "New Project":
                Main.NewProject("Untitled Project");
                break;
            case "Report a Bug":
                System.Diagnostics.Process.Start("https://github.com/SimonStorlSchulke/FNode/issues/new/choose");
                break;
            case "Help (F1)":
                GetNode<Popup>(NPPUHotkey).PopupCentered();
                break;
            default:
                break;
        }
    }

}
