using System.IO;
using Godot;
using System;

public class FNodeJoinStrings : FNode
{
    public FNodeJoinStrings() {
        HintTooltip = "Join multiple Strings separated by the a Separator String.\nTo Split Lines, instert [LINEBREAK] somwhere in the Separator String";
        category = "String";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Separator", new FInputString(this, initialValue: "[LINEBREAK]")},
            {"String1", new FInputString(this)},
            {"String2", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "String", new FOutputString(this, delegate() 
            {
                string str = "";
                string sep = inputs["Separator"].Get() as string;
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...
                int i = 0;
                foreach (var item in inputs) {
                    if (i!=0) {
                        str += i < inputs.Count-1 ? item.Value.Get() as string + sep : item.Value.Get() as string; //TODO Use String.Join;
                    }
                    i++;
                }
                return str;
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
        inputs.Add("String" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "String"+(inputs.Count), inputs["String" + (inputs.Count)]);
        MoveChild(GetNode("PlusButton"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
