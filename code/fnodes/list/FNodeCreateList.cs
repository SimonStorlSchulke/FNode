using Godot;
using System;
using System.Collections.Generic;

public partial class FNodeCreateList : FNode, IFNodeVarInputSize

{
    public FNodeCreateList() {
        TooltipText = "Join multiple items of variable type to a List";
        category = "List";        

        IdxReset();
        inputs = new Dictionary<string, FInput>() {
            {"Item 1", new FInput(this)},
            {"Item 2", new FInput(this)},
        };

        IdxReset();
        outputs = new Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() 
            {
                List<object> list = new();
                foreach (var item in inputs) {
                    list.Add(item.Value.Get<object>());
                }
                return list;
            })},
        };
    }

    public override void _Ready() {
        base._Ready();
        HBoxContainer HBButtons = new HBoxContainer();
        HBButtons.Name = "HBButtons";
        nButton plusButton = new nButton("+", this, AddInput, name: "PlusButton");
        nButton minusButton = new nButton("-", this, RemoveInput, name: "MinusButton");
        plusButton.SizeFlagsHorizontal = minusButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        HBButtons.AddChildren(plusButton, minusButton);
        AddChild(HBButtons);
    }

    void AddInput() {
        inputs.Add("Item " + (inputs.Count+1), new FInput(this, inputs.Count));
        UIBuilder.AddInputUI(this, "Item "+(inputs.Count), inputs["Item " + (inputs.Count-2)]);
        MoveChild(GetNode("HBButtons"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.White, false, 0, Colors.White, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveInput() {
        if (GetChildCount() < 5) {
            return;
        }

        inputs.Remove("Item " + (inputs.Count));
        Node rmNode = GetChild(GetChildCount()-2);
        RemoveChild(rmNode);
        rmNode.QueueFree();
        Size = CustomMinimumSize;
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    public void SetInputSize(int size) {
        if (size > inputs.Count) {
            for (int i = 0; i < (size+1)-(inputs.Count-1); i++) {
                AddInput();
            }
        }
    }
}
