using Godot;

public class TextEditor : AcceptDialog
{
    public static TextEditor inst;
    FInputString connectedInput;

    public override void _Ready() {
        inst = this;
    }

    public void ShowEditor(FNode connectedNode, string connectedSlotName) {
        connectedInput = Main.Inst.CurrentProject.NodeTree.GetNode<FNode>(connectedNode.Name)
            .inputs[connectedSlotName] as FInputString; //Dangerous...
        connectedInput.UpdateDefaultValueFromUI();
        GetNode<TextEdit>("TE").Text = connectedInput.DefaultValue as string;
        PopupCentered();
    }

    public void OnPopupHide() {
        connectedInput.DefaultValue = GetNode<TextEdit>("TE").Text;
        connectedInput.UpdateUIFromValue(connectedInput.DefaultValue);
    }
}
