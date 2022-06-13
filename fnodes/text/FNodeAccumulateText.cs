using System.IO;
using Godot;
using System;

public class FNodeAccumulateText : FNode
{
    string accumulatedString = "";
    public FNodeAccumulateText() {
        HintTooltip = "Accumulates Text from all Iterations (when looping ofer a File Stack)\nand combines them using the Separator";
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
                
                if (!Main.inst.IsConnected(nameof(Main.StartParsing), this, nameof(ResetString))) {
                    Main.inst.Connect(nameof(Main.StartParsing), this, nameof(ResetString)); //Doing this here because of process order (instance of main is initialized after Nodes)
                }
                
                string sep = inputs["Separator"].Get() as string;
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...
                
                accumulatedString += Project.idxEval < Project.maxNumFiles-1 ? inputs["Text"].Get() as string + sep : inputs["Text"].Get() as string; //TODO Use String.Join;
                
                return accumulatedString;
            })},
        };
    }
    public void ResetString() {
        accumulatedString = "";
    }
}

