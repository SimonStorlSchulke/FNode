using System.IO;
using Godot;
using System;

public class FNodeAccumulateNumber : FNode
{
    float accumulatedFloat = 0;
    public FNodeAccumulateNumber() {
        HintTooltip = "Accumulates the Input Numbers from all Iterations \nand combines them using the Separator";
        category = "Math";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Number", new FInputFloat(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Number", new FOutputFloat(this, delegate() {
                                
                accumulatedFloat += inputs["Number"].Get<float>(); //TODO Use String.Join;
                return accumulatedFloat;
            })},
        };
    }
    public override void OnBeforeEvaluation() {
        GD.Print("BFE");
        accumulatedFloat = 0;
    }
}

