using Godot;
using System;

public class FNodeAccumulateText : FNode
{
    string accumulatedString = "";
    string selectedOption;
    int iterations = (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);
    string separator;
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
                return accumulatedString;
            })},
        };
    }

    public override void OnNextIteration() {
        if (selectedOption == "Incremental") {
            string toAdd = inputs["Text"].Get<string>();
            if (toAdd != "") {
                accumulatedString += Project.IdxEval < iterations-1 ? toAdd + separator : toAdd;
            }
        }
    }

    public override void OnBeforeEvaluation() {
        accumulatedString = "";
        selectedOption = GetSelectedOption("Mode");

        separator = inputs["Separator"].Get<string>();
        separator = separator.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...

        if (selectedOption == "Instant") {
            iterations = (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);
            for (int i = 0; i < iterations; i++) {

                // Would be enough formost cases but doesn't guarantee all important nodes run OnNextIteration...
                //inputs["Value"].connectedTo.owner.OnNextIteration();

                // so we brute force all RunBeforeIterationGroup-Nodes instead...
                GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.RunBeforeIterationGroup, nameof(FNode.OnNextIteration));

                string toAdd = inputs["Text"].Get<string>();
            if (toAdd == "") {
                GD.Print("AAyo");
            }
            if (toAdd != "") {
                accumulatedString += Project.IdxEval < iterations-1 ? toAdd + separator : toAdd;
            }
                Project.IdxEval++;
            }
            Project.IdxEval = 0;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        AddOptionEnum(
            "Mode",

            new string[] {
                "Instant",
                "Incremental",
            },
            "If set to instant, the final Number will be evaluated at the first iteration and returned with each one afterwards.\nWhen set to incremental, it fetches the input value for each iteration and adds it to the list.");
    }
}

