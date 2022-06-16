using Godot;
using System;

public class FNodeJoinTexts : FNode
{
    public FNodeJoinTexts() {
        HintTooltip = "Join multiple Strings separated by the a Separator String.\nTo Split Lines, instert [LINEBREAK] somwhere in the Separator String";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Filter", new FInputBool(this, initialValue: true)},
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

                if (!(bool)inputs["Filter"].Get()) {
                    return str;
                }

                string sep = inputs["Separator"].Get() as string;
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...
                int i = 0;
                foreach (var item in inputs) {
                    if (i!=0) {
                        str += i < inputs.Count-3 ? item.Value.Get() as string + sep : item.Value.Get() as string;
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
        Button plusButton = new Button();
        Button minusButton = new Button();
        plusButton.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        minusButton.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        HBButtons.Name = "HBButtons";
        plusButton.Name = "PlusButton";
        minusButton.Name = "MinusButton";
        plusButton.Text = "+";
        minusButton.Text = "-";
        plusButton.Connect("pressed", this, nameof(AddLine));
        minusButton.Connect("pressed", this, nameof(RemoveLine));
        HBButtons.AddChild(plusButton);
        HBButtons.AddChild(minusButton);
        AddChild(HBButtons);
    }

    void AddLine() {
        inputs.Add("Text" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "Text"+(inputs.Count), inputs["Text" + (inputs.Count)]);
        MoveChild(GetNode("HBButtons"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveLine() {
        if (GetChildCount() < 6) {
            return;
        }
        inputs.Remove("Text" + (inputs.Count));
        Node rmNode = GetChild(GetChildCount()-2);
        RemoveChild(rmNode);
        rmNode.QueueFree();
        RectSize = RectMinSize;
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
