using System.IO;
using Godot;
using System;

public class FNodeAccumulateString : FNode
{
    string accumulatedString = "";
    public FNodeAccumulateString() {
        HintTooltip = "Accumulates Strings from all Iterations (when looping ofer a File Stack)\nand combines them using the Separator";
        category = "String";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Separator", new FInputString(this)},
            {"String", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "String", new FOutputString(this, delegate() {
                
                if (!Main.inst.IsConnected(nameof(Main.StartParsing), this, nameof(ResetString))) {
                    Main.inst.Connect(nameof(Main.StartParsing), this, nameof(ResetString)); //Doing this here because of process order (instance of main is initialized after Nodes)
                }
                
                string sep = inputs["Separator"].Get() as string;
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...
                
                accumulatedString += Project.idxEval < Project.maxNumFiles-1 ? inputs["String"].Get() as string + sep : inputs["String"].Get() as string; //TODO Use String.Join;
                
                return accumulatedString;
            })},
        };
    }
    public void ResetString() {
        accumulatedString = "";
    }

    public override void _Ready()
    {
        base._Ready();
        (GetChild(1).GetChild(1) as LineEdit).Text = "[LINEBREAK]"; //Set Default Value to Linebreak
    }
}

