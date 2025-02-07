using Godot;
using System;

public partial class FNodeList : FNode
{
    public FNodeList() {
        TooltipText = "Generate list of values";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                return inputs["List"].Get<Godot.Collections.Array>();
            })},
            {
            "List Count", new FOutputInt(this, delegate() {
                return inputs["List"].Get<Godot.Collections.Array>().Count;
            })},
        };
    }
}
