using System.IO;
using Godot;
using System;

public class FNodeAccumulateNumber : FNode
{
    float accumulatedFloat = 0;
    public FNodeAccumulateNumber() {
        HintTooltip = "Accumulates Strings from all Iterations (when looping ofer a File Stack)\nand combines them using the Separator";
        category = "Math";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Number", new FInputFloat(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Accumulated Number", new FOutputFloat(this, delegate() {
                
                if (!Main.inst.IsConnected(nameof(Main.StartParsing), this, nameof(ResetFloat))) {
                    Main.inst.Connect(nameof(Main.StartParsing), this, nameof(ResetFloat)); //Doing this here because of process order (instance of main is initialized after Nodes)
                }
                                
                accumulatedFloat += (float)inputs["Number"].Get(); //TODO Use String.Join;
                return accumulatedFloat;
            })},
        };
    }
    public void ResetFloat() {
        accumulatedFloat = 0;
    }
}

