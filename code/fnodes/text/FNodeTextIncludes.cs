using System.IO;
using Godot;
using System;

public class FNodeTextIncludes : FNode
{
    OptionButton ob;
    public FNodeTextIncludes() {
        HintTooltip = "Returns true if the given String includes one of the given strings";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Includes1", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Includes", new FOutputBool(this, delegate() 
            {
                string str = inputs["Text"].Get<string>();
                int i = 0;
                bool includesAll = true;
                foreach (var item in inputs) {
                    if (i!=0) {
                        bool contains = str.Contains(item.Value.Get<string>());
                        if (contains) {
                            if (ob.Selected == 0)  {
                                return true;
                            }
                        }
                        if (!contains) {
                            includesAll = false;
                        }
                    }
                    i++;
                }
                return includesAll;
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

        ob = new OptionButton();
        ob.Name = "ob";
        ob.AddItem("Includes One");
        ob.AddItem("Includes All");
        AddChild(ob);
    }

    void AddLine() {
        inputs.Add("Includes" + (inputs.Count+1), new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, "Includes"+(inputs.Count), inputs["Includes" + (inputs.Count)]);
        MoveChild(GetNode("ob"), GetChildCount()-1);
        MoveChild(GetNode("PlusButton"), GetChildCount()-2);
        SetSlot(GetChildCount()-3, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
        SetSlot(GetChildCount()-2, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
