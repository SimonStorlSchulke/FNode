using Godot;

///<summary>Editor for Text-inputs on FNodes</summary>
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
        // COMMENTING THIS MIGHT HAVE HAD SIDE EFFECTS
        // connectedInput.DefaultValue = GetNode<TextEdit>("TE").Text;
        connectedInput.UpdateUIFromValue(GetNode<TextEdit>("TE").Text);
    }
}
