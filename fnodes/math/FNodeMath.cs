using System.IO;
using Godot;
using System;

public class FNodeMath : FNode
{
    OptionButton ob;
    public FNodeMath() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";      

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Val1", new FInputFloat(this)},
            {"Val2", new FInputFloat(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutputFloat(this, delegate() {
                float val1 = (float)inputs["Val1"].Get();
                float val2 = (float)inputs["Val2"].Get();
                switch (ob.Selected)
                {
                    case 0:
                        return val1 + val2;
                    case 1:
                        return val1 - val2;
                    case 2:
                        return val1 * val2;
                    case 3:
                        return val1 / val2;
                    case 4:
                        return Mathf.Pow(val1, val2);
                    case 5:
                        return Mathf.Max(val1, val2);
                    case 6:
                        return Mathf.Min(val1, val2);
                    case 7:
                        return val1 > val2 ? 1f : 0f;
                    case 8:
                        return val1 < val2 ? 1f : 0f;
                     case 9:
                        return Mathf.Abs(val1);
                    case 10:
                        return Mathf.Round(val1);
                    default:
                        return 0f;
                }
            })},
        };
    }

    public override void _Ready()
    {
        base._Ready();
        ob = new OptionButton();
        ob.AddItem("Add");
        ob.AddItem("Subtract");
        ob.AddItem("Multiply");
        ob.AddItem("Divide");
        ob.AddItem("Pow");
        ob.AddItem("Max");
        ob.AddItem("Min");
        ob.AddItem("Greater Than");
        ob.AddItem("Less Than");
        ob.AddItem("Abs (only uses Val1)");
        ob.AddItem("Round (only uses Val1)");
        AddChild(ob);
    }
}
