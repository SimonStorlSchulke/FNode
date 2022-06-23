using System.IO;
using Godot;
using System;

public class FNodeAccumulateText : FNode
{
    string accumulatedString = "";
    public FNodeAccumulateText() {
        HintTooltip = "Accumulates Text from all Iterations \nand combines them using the Separator";
        category = "Text";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Separator", new FInputString(this, description: "This Text will be added between each accumulated Text", initialValue: "[LINEBREAK]")},
            {"Text", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
        
                string sep = inputs["Separator"].Get<string>();
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...

                int iterations = (int)Math.Max(Main.inst.currentProject.spIterations.Value, Project.maxNumFiles);
                
                accumulatedString += Project.idxEval < iterations-1 ? inputs["Text"].Get<string>() + sep : inputs["Text"].Get<string>(); //TODO Use String.Join;
                
                return accumulatedString;
            })},
        };
    }
    public override void OnBeforeEvaluation() {
        accumulatedString = "";
    }
}

