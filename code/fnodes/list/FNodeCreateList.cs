using Godot;
using System;

public class FNodeCreateList : FNode, IFNodeVarInputSize

{
    public FNodeCreateList() {
        HintTooltip = "Join multiple items of variable type to a List";
        category = "List";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Item 1", new FInput(this)},
            {"Item 2", new FInput(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() 
            {
                var list = new Godot.Collections.Array();
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
        nButton plusButton = new nButton("+", this, nameof(AddInput), name: "PlusButton");
        nButton minusButton = new nButton("-", this, nameof(RemoveInput), name: "MinusButton");
        plusButton.SizeFlagsHorizontal = minusButton.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
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
        RectSize = RectMinSize;
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
