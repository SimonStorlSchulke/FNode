using Godot;
using System;

public class FNodeJoinPaths : FNode
{
    public FNodeJoinPaths() {
        HintTooltip = "Join multiple Strings separated by the a Separator String.\nTo Split Lines, instert [LINEBREAK] somwhere in the Separator String";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path1", new FInputString(this)},
            {"Path2", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Path", new FOutputString(this, delegate() 
            {
                string str = "";// FileUtil.JoinPaths((string)inputs["Path1"].Get<object>(), (string)inputs["Path2"].Get<object>());
                int i = 0;
                foreach (var item in inputs) {
                        str = FileUtil.JoinPaths(str, (string)item.Value.Get<object>());
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
        inputs.Add("Path" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "Path"+(inputs.Count), inputs["Path" + (inputs.Count)]);
        MoveChild(GetNode("HBButtons"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveLine() {
        if (GetChildCount() < 5) {
            return;
        }
        inputs.Remove("Path" + (inputs.Count));
        Node rmNode = GetChild(GetChildCount()-2);
        RemoveChild(rmNode);
        rmNode.QueueFree();
        RectSize = RectMinSize;
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
