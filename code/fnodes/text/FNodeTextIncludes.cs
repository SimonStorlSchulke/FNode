using System.IO;
using Godot;
using System;
using  static GdExtensions;

public class FNodeTextIncludes : FNode, IFNodeVarInputSize
{
    OptionButton ob;
    public FNodeTextIncludes() {
        HintTooltip = "Returns true if the given String includes one of the given strings";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Case Sensitive", new FInputBool(this)},
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
                    if (i>1) {
                        bool contains = inputs["Case Sensitive"].Get<bool>() ? 
                            DevUtil.StringContains(str, item.Value.Get<string>(), StringComparison.Ordinal) :
                            DevUtil.StringContains(str, item.Value.Get<string>(), StringComparison.OrdinalIgnoreCase); 
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
        HBoxContainer HBPlusMinus = new HBoxContainer();
        HBPlusMinus.Name = "HBPlusMinus";

        Button plusButton = new Button();
        Button minusButton = new Button();
        plusButton.Name = "PlusButton";
        minusButton.Name = "MinusButton";
        HBPlusMinus.AddChildren(plusButton, minusButton);
        plusButton.Text = "+";
        minusButton.Text = "-";
        plusButton.Connect("pressed", this, nameof(AddLine));
        minusButton.Connect("pressed", this, nameof(RemoveLine));
        AddChild(HBPlusMinus);

        ob = new OptionButton();
        ob.Name = "ob";
        ob.AddItem("Includes One");
        ob.AddItem("Includes All");
        AddChild(ob);
    }

    void AddLine() {
        string inpName = "Includes" + (inputs.Count-1);
        inputs.Add(inpName, new FInputString(this, inputs.Count));
        UIUtil.AddInputUI(this, inpName, inputs[inpName]);
        MoveChild(GetNode("ob"), GetChildCount()-1);
        MoveChild(GetNode("HBPlusMinus"), GetChildCount()-2);

        SetSlot(GetChildCount()-3, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
        SetSlot(GetChildCount()-2, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    void RemoveLine() {
        if (inputs.Count < 4) {
            return;
        }
        string inpName = "Includes" + (inputs.Count-2);
        inputs.Remove(inpName);
        GetNode(inpName).QueueFree();

        SetSlot(GetChildCount()-4, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-2, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
        SetSlot(GetChildCount()-3, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }

    public void SetInputSize(int size) {

        int toAdd = (size - inputs.Count);

        if (size > inputs.Count) {
            for (int i = 0; i < toAdd; i++) {
                AddLine();
            }
        }
    }
}
