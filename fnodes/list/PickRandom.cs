using Godot;
using System;

public class FNodePickRandom : FNode
{
    public FNodePickRandom() {
        HintTooltip = "Retrieve a random Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List1", new FInputList(this)},
            {"Seed", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Item", new FOutput(this, delegate() {
                int idx = (int)inputs["Index"].Get();
                Godot.Collections.Array arr = (Godot.Collections.Array)inputs["List"].Get();
                if (arr == null) {
                    return null;
                }
                if (idx < arr.Count) {
                    return arr[idx];
                } else {
                    return null;
                }
            })},
        };
    }
}
