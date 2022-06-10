using System.IO;
using Godot;
using System;

public class FNodeJoinPaths : FNode
{
    public FNodeJoinPaths() {
        HintTooltip = "Join multiple Strings separated by the a Separator String.\nTo Split Lines, instert [LINEBREAK] somwhere in the Separator String";
        category = "String";        

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
                string path = (string)inputs["Path1"].Get();
                int i = 0;
                foreach (var item in inputs) {
                    if (i!=0) {
                        path = System.IO.Path.Combine(path, item.Value.Get() as string);
                    }
                    i++;
                }
                return path;
            })},
        };
    }

    public override void _Ready()
    {
        base._Ready();
        Button plusButton = new Button();
        plusButton.Name = "PlusButton";
        plusButton.Text = "+";
        plusButton.Connect("pressed", this, nameof(AddLine));
        AddChild(plusButton);
    }

    void AddLine() {
        inputs.Add("Path" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "Path"+(inputs.Count), inputs["Path" + (inputs.Count)]);
        MoveChild(GetNode("PlusButton"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
