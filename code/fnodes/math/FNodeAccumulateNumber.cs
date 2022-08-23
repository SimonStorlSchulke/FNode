using System.IO;
using Godot;
using System;

public class FNodeAccumulateNumber : FNode
{
    float accumulatedFloat = 0;
    string selectedOption;
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
                return accumulatedFloat;
            })},
        };
    }

    public override void OnNextIteration() {
        if (selectedOption == "Incremental") {
            accumulatedFloat += inputs["Number"].Get<float>();
        }
    }

    public override void OnBeforeEvaluation() {
        accumulatedFloat = 0;
        selectedOption = GetSelectedOption("Mode");

        if (selectedOption == "Instant") {
            int iterations = (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);
            for (int i = 0; i < iterations; i++) {

                // Would be enough formost cases but doesn't guarantee all important nodes run OnNextIteration...
                //inputs["Value"].connectedTo.owner.OnNextIteration();

                // so we brute force all RunBeforeIterationGroup-Nodes instead...
                GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.RunBeforeIterationGroup, nameof(FNode.OnNextIteration));

                accumulatedFloat += inputs["Number"].Get<float>();
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

