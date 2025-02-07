using System.IO;
using Godot;
using System;

public partial class FNodeFloatNumber : FNode
{
    public FNodeFloatNumber() {
        TooltipText = "An int (whole) Number";
        category = "Math";      

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Value", new FInputFloat(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutputFloat(this, delegate() {
                return inputs["Value"].Get<float>();
            })},
        };
    }
}
