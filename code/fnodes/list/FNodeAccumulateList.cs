using Godot;
using System;

public class FNodeAccumulateList : FNode
{
    Godot.Collections.Array accumulatedList = new Godot.Collections.Array();
    Godot.Collections.Array emptyArr = new Godot.Collections.Array();

    public FNodeAccumulateList()
    {   
        HintTooltip = "Collects the input value for each iteration and adds it to the List";
        category = "List";


        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Value", new FInput(this)},
            {"just last iteration", new FInputBool(this, initialValue: true, description: "If this is checked, wait for all iterations to finish until returning the complete List at the last iteration")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                if ((bool)inputs["just last iteration"].Get()) {
                    int iterations = (int)Math.Max(Main.inst.currentProject.spIterations.Value, Project.maxNumFiles);
                        return (Project.idxEval > (iterations-2)) ? accumulatedList : emptyArr;
                }
                return accumulatedList;
            })},
        };
    }

    public override void OnNextIteration() {
        accumulatedList.Add(inputs["Value"].Get());
    }

    public override void OnBeforeEvaluation() {
       accumulatedList = new Godot.Collections.Array();
    }
}

