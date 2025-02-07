using System.IO;
using Godot;
using System;

public partial class FNodeIntNumber : FNode
{
    public FNodeIntNumber() {
        TooltipText = "An int (whole) Number";
        category = "Math";      

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Value", new FInputInt(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutputInt(this, delegate() {
                return inputs["Value"].Get<int>();
            })},
        };
    }
}
