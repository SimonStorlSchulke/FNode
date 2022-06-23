using Godot;
using System;

public class FNodeJoinList : FNode
{
    public FNodeJoinList() {
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
                return (Godot.Collections.Array)inputs["List1"].Get<object>() + (Godot.Collections.Array)inputs["List2"].Get<object>();
            })},
        };
    }
}
