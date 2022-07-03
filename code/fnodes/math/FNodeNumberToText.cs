using System.IO;
using Godot;
using System;

public class FNodeNumberToText : FNode
{
    public FNodeNumberToText() {
        HintTooltip = "Convert Numbers to text. The input number can als be an int. if you want 7 to be 007, enter 000 as Format.\nIf you want four decimals, enter 0.0000";
        category = "Math";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Number", new FInputFloat(this)},
            {"Format", new FInputString(this, initialValue: "0.00", 
                description: "if you want 7 to be 007, enter 000 as Format.\nIf you want four decimals, enter 0.0000 you can also add pre-/ postfixes for example _0000 or 0.0m")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
                string str = "WRONG FORMAT";
                try {
                    str = (inputs["Number"].Get<float>()).ToString(
                        inputs["Format"].Get<string>(),
                        System.Globalization.CultureInfo.InvariantCulture);
                } catch (System.Exception e) {
                    Errorlog.Log(this, e);
                }

                return str;
            })},
        };
    }
}
