using Godot;
using System;

public class FNodeList : FNode
{
    public FNodeList() {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                return (Godot.Collections.Array)inputs["List"].Get();
            })},
            {
            "List Count", new FOutputInt(this, delegate() {
                return ((Godot.Collections.Array)inputs["List"].Get()).Count;
            })},
        };
    }
}
