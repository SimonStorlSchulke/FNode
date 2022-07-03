using Godot;
using System;

public class FNodeJoinList : FNode
{
    public FNodeJoinList() {
        HintTooltip = "Join two lists into one.";
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
