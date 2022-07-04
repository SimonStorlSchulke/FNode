using Godot;
using System;

public class FNodeAccumulateList : FNode
{
    Godot.Collections.Array accumulatedList = new Godot.Collections.Array();
    Godot.Collections.Array emptyArr = new Godot.Collections.Array();
    string selectedOption;

    public FNodeAccumulateList()
    {   
        HintTooltip = "Collects the input value for each iteration and adds it to the List";
        category = "List";


        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Value", new FInput(this)},
            {"Custom Iterations", new FInputInt(this, min: 0, description: "If this is not 0, use it as the desired iterations for this List (only works in instant Mode)")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                return accumulatedList;
            })},
        };
    }

    public override void OnNextIteration() {
        if (selectedOption == "Incremental") {
            accumulatedList.Add(inputs["Value"].Get<object>());
        }
    }

    public override void OnBeforeEvaluation() {
        accumulatedList = new Godot.Collections.Array();
        selectedOption = GetSelectedOption("Mode");
        if (selectedOption == "Instant") {
            int iterations = (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);
            int customIts = inputs["Custom Iterations"].Get<int>();
            if (customIts != 0) {
                iterations = customIts;
            }
            for (int i = 0; i < iterations; i++) {

                // Would be enough formost cases but doesn't guarantee all important nodes run OnNextIteration...
                //inputs["Value"].connectedTo.owner.OnNextIteration();

                // so we brute force all RunBeforeIterationGroup-Nodes instead...
                GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.RunBeforeIterationGroup, nameof(FNode.OnNextIteration));

                accumulatedList.Add(inputs["Value"].Get<object>());
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
            "If set to instant, the List will be generated at the first iteration and returned with each one afterwards.\nWhen set to incremental, it fetches the input value for each iteration and adds it to the list.");
    }
}

