using Godot;
using System;

public class FNodeSubList : FNode
{
    public FNodeSubList() {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List1", new FInputList(this)},
            {"List2", new FInputList(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                return inputs["List1"].Get<Godot.Collections.Array>() + inputs["List2"].Get<Godot.Collections.Array>();
            })},
        };
    }
}
