using Godot;
using System;

public class FNodeGetListItem : FNode
{
    public FNodeGetListItem() {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "Math";      

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
            {"Index", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutput(this, delegate() {
                int idx = (int)inputs["Index"].Get();
                Godot.Collections.Array arr = (Godot.Collections.Array)inputs["List"].Get();
                if (idx < arr.Count) {
                    return arr[idx];
                } else {
                    return null;
                }
            })},
            {
            "Testlist", new FOutputList(this, delegate() {
                return new Godot.Collections.Array("1", 32, "3");
            })},
        };
    }
}
