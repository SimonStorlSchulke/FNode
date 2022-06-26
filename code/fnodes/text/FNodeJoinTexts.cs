using Godot;
using System;

public class FNodeJoinTexts : FNode, IFNodeVarInputSize
{
    public FNodeJoinTexts() {
        HintTooltip = "Join multiple Strings separated by the a Separator String.\nTo Split Lines, instert [LINEBREAK] somwhere in the Separator String";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Toggle", new FInputBool(this, initialValue: true)},
            {"Separator", new FInputString(this, initialValue: "[LINEBREAK]")},
            {"Text1", new FInputString(this)},
            {"Text2", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() 
            {
                string str = "";

                if (!inputs["Toggle"].Get<bool>()) {
                    return inputs["Text1"].Get<string>();
                }

                string sep = inputs["Separator"].Get<string>();
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...
                int i = 0;
                foreach (var item in inputs) {
                    if (i>1) {
                        str += i < inputs.Count-1 ? item.Value.Get<string>() + sep : item.Value.Get<string>();
                    }
                    i++;
                }
                return str;
            })},
        };
    }

    public override void _Ready() {
        base._Ready();
        HBoxContainer HBButtons = new HBoxContainer();
        nButton plusButton = new nButton("+", this, nameof(AddInput), name: "PlusButton");
        nButton minusButton = new nButton("-", this, nameof(RemoveInput), name: "MinusButton");
        plusButton.SizeFlagsHorizontal = minusButton.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        HBButtons.AddChildren(plusButton, minusButton);
        AddChild(HBButtons);
    }

    void AddInput() {
        inputs.Add("Text" + (inputs.Count-1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "Text"+(inputs.Count-2), inputs["Text" + (inputs.Count-2)]);
        MoveChild(GetNode("HBButtons"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveInput() {
        if (GetChildCount() < 7) {
            return;
        }

        inputs.Remove("Text" + (inputs.Count-2));
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
