using Godot;
using System;

public class TextEditor : AcceptDialog
{
    public static TextEditor inst;
    FInputString connectedInput;

    public override void _Ready() {
        inst = this;
    }

    public void ShowEditor(FNode connectedNode, string connectedSlotName) {
        connectedInput = Main.inst.currentProject.NodeTree.GetNode<FNode>(connectedNode.Name)
            .inputs[connectedSlotName] as FInputString; //Dangerous...
        connectedInput.UpdateDefaultValueFromUI();
        GetNode<TextEdit>("TE").Text = connectedInput.defaultValue as string;
        PopupCentered();
    }

    public void OnPopupHide() {
        connectedInput.defaultValue = GetNode<TextEdit>("TE").Text;
        connectedInput.UpdateUIFromValue(connectedInput.defaultValue);
    }
}
