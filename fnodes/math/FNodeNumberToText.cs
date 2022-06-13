using System.IO;
using Godot;
using System;

public class FNodeNumberToText : FNode
{
    public FNodeNumberToText() {
        HintTooltip = "";
        category = "Math";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Number", new FInputFloat(this)},
            {"Length", new FInputInt(this, initialValue: 4)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
                return "Project.idxEval";
            })},
        };
    }
}
