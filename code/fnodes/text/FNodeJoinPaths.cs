using Godot;
using System;

public partial class FNodeJoinPaths : FNode, IFNodeVarInputSize
{
    public FNodeJoinPaths() {
        TooltipText = "Join multiple filepaths";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path1", new FInputString(this)},
            {"Path2", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Path3D", new FOutputString(this, delegate() 
            {
                string str = "";
                int i = 0;
                foreach (var item in inputs) {
                        str = FileUtil.JoinPaths(str, item.Value.Get<string>());
                    i++;
                }
                return str;
            })},
        };
    }

    public override void _Ready() {
        base._Ready();
        HBoxContainer HBButtons = new HBoxContainer();
        nButton plus = new nButton("+", this, AddLine, name: "PlusButton");
        nButton minus = new nButton("+", this, RemoveLine, name: "MinusButton");
        plus.SizeFlagsHorizontal = minus.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        HBButtons.Name = "HBButtons";
        HBButtons.AddChildren(plus, minus);
        AddChild(HBButtons);
    }

    void AddLine() {
        inputs.Add("Path3D" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIBuilder.AddInputUI(this, "Path3D"+(inputs.Count), inputs["Path3D" + (inputs.Count)]);
        MoveChild(GetNode("HBButtons"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveLine() {
        if (GetChildCount() < 5) {
            return;
        }
        inputs.Remove("Path3D" + (inputs.Count));
        Node rmNode = GetChild(GetChildCount()-2);
        RemoveChild(rmNode);
        rmNode.QueueFree();
        Size = CustomMinimumSize;
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    public void SetInputSize(int size) {
        if (size > inputs.Count) {
            for (int i = 0; i < (size+1)-(inputs.Count-1); i++) {
                AddLine();
            }
        }
    }
}
