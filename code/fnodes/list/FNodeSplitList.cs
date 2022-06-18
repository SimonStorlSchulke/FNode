using Godot;
using System;

public class FNodeSplitList : FNode
{
    public FNodeSplitList() {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
            {"At Index", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List1", new FOutputList(this, delegate() {
                var arr = (Godot.Collections.Array)inputs["List"].Get();
                var subArr1 = new Godot.Collections.Array();
                int uiAtIdx = (int)inputs["At Index"].Get();
                int atIdx = uiAtIdx > arr.Count ? arr.Count : uiAtIdx;

                for (int i = 0; i < atIdx; i++) {
                    subArr1.Add(arr[i]);
                }
                
                return subArr1;
            })},
            {
            "List2", new FOutputList(this, delegate() {
                var arr = (Godot.Collections.Array)inputs["List"].Get();
                var subArr1 = new Godot.Collections.Array();

                int uiAtIdx = (int)inputs["At Index"].Get();

                if (uiAtIdx > arr.Count) {
                    return subArr1;
                }

                for (int i = uiAtIdx; i < arr.Count; i++) {
                    subArr1.Add(arr[i]);
                }
                
                return subArr1;
            })},
        };
    }
}
