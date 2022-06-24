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
            {"Toggle", new FInputBool(this, initialValue: true)},
            {"Start", new FInputInt(this, min: 0)},
            {"End", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
                string str = inputs["Text"].Get<string>();

                if (!inputs["Toggle"].Get<bool>()) {
                    return str;
                }
                
                int start = inputs["Start"].Get<int>();
                int end = inputs["End"].Get<int>();
                if (start + end > str.Length) {
                    return str;
                }
                if (start < 0) start = 0;
                if (end < 0) end = 0;

                str = str.Substring(start, str.Length - start - end);
                return str;
            })},
        };
    }
}
