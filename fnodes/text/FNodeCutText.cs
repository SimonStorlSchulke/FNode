using Godot;
using System;

public class FNodeCutText : FNode
{
    public FNodeCutText() {
        HintTooltip = "Cut Characters from Start and End of a Text";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Filter", new FInputBool(this, initialValue: true)},
            {"Start", new FInputInt(this)},
            {"End", new FInputInt(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
                string str = (string)inputs["Text"].Get();

                if (!(bool)inputs["Filter"].Get()) {
                    return str;
                }
                
                int start = (int)inputs["Start"].Get();
                int end = (int)inputs["End"].Get();
                if (start + end > str.Length) {
                    return str;
                }
                str = str.Substring(start, str.Length - start - end);
                return str;
            })},
        };
    }
}
