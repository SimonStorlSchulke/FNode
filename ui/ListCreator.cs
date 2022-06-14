using Godot;
using System;

public class ListCreator : AcceptDialog {
    static Godot.Collections.Array list;
    [Export] NodePath NPTypeSelector;
    OptionButton typeSelector;
    [Export] NodePath NPCountSelector;
    SpinBox countSelector;
    [Export] NodePath NPItemsBox;
    VBoxContainer itemsBox;
    public static ListCreator inst;
    public static FInputList connectedListInput; //TODO hmmm... how? FNip can't be passed by signal..

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        inst = this;
        typeSelector = GetNode<OptionButton>(NPTypeSelector);
        countSelector = GetNode<SpinBox>(NPCountSelector);
        itemsBox = GetNode<VBoxContainer>(NPItemsBox);
        countSelector.Connect("value_changed", this, nameof(SetItemCount));
    }

    public void ShowCreator(Godot.Collections.Array listInput, string connectedNodeName, string connectedSlotName) {
        list = listInput;
        //GD.Print($"{connectedNodeName} slotname: {connectedSlotName}");
        connectedListInput = Main.inst.currentProject.NodeTree.GetNode<FNode>(connectedNodeName).inputs[connectedSlotName] as FInputList; //Dangerous...
        for (int i = 0; i < (int)inst.countSelector.Value; i++) {
            inst.itemsBox.AddChild(new SpinBox());
        }
        inst.PopupCentered();
    }

    public void OnPopupHide() {
        //inp
        foreach (Node c in itemsBox.GetChildren()) {
            c.QueueFree();
        }
        connectedListInput.defaultValue = new Godot.Collections.Array(){"TEST", "YAY"};
        GD.Print(((Godot.Collections.Array)connectedListInput.defaultValue)[1]);
    }

    public void SetItemCount(int count) {
        int diff = count - itemsBox.GetChildCount();

        if (diff > 0) {
            for (int i = 0; i < diff; i++) {
                itemsBox.AddChild(new SpinBox()); 
            }
        } else if(diff < 0) {
            for (int i = 0; i < -diff; i++) {
                itemsBox.GetChild(itemsBox.GetChildCount()-1).QueueFree();
            }
        }
    }

}
