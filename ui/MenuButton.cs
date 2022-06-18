using Godot;

public class MenuButton : Godot.MenuButton
{
    [Export] NodePath NPIO;
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
            default:
                break;
        }
    }

}
