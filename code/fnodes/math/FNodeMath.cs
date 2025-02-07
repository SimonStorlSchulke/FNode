using System.IO;
using Godot;
using System;

public partial class FNodeMath : FNode
{
    public FNodeMath() {
        TooltipText = "Multiple nummerical math operations";
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
                float val1;
                float val2;
                try {
                    val1 = inputs["Val1"].Get<float>();
                } catch {
                    val1 = 0;
                }
                try {
                    val2 = inputs["Val2"].Get<float>();
                } catch {
                    val2 = 0;
                }
                string selectedOption = GetSelectedOption("Mode");
                switch (selectedOption)
                {
                    case "Add":
                        return val1 + val2;
                    case "Subtract":
                        return val1 - val2;
                    case "Multiply":
                        return val1 * val2;
                    case "Divide":
                        return val1 / val2;
                    case "Pow":
                        return Mathf.Pow(val1, val2);
                    case "Max":
                        return Mathf.Max(val1, val2);
                    case "Min":
                        return Mathf.Min(val1, val2);
                    case "Greater Than":
                        return val1 > val2 ? 1f : 0f;
                    case "Less Than":
                        return val1 < val2 ? 1f : 0f;
                     case "Abs (only uses Val1)":
                        return Mathf.Abs(val1);
                    case "Round (only uses Val1)":
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
        AddOptionEnum(
            "Mode",

            new string[] {
                "Add",
                "Subtract",
                "Multiply",
                "Divide",
                "Pow",
                "Max",
                "Min",
                "Greater Than",
                "Less Than",
                "Abs (only uses Val1)",
                "Round (only uses Val1)"
            });
    }
}
