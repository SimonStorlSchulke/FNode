using Godot;
using System;

public class FNodePickRandom : FNode
{
    ulong seedAdds = 0;

    public FNodePickRandom() {
        HintTooltip = "Retrieve a random Item from a List. The output is of a variable Type.";
        category = "List";

        RandomNumberGenerator rng = new RandomNumberGenerator();

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
            {"Seed", new FInputInt(this, initialValue: -1, min: -1, description: "If you put -1 here, the seed will be randomized")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Item", new FOutput(this, delegate() {

                int uiSeed = (int)inputs["Seed"].Get();
                if (uiSeed == -1) {
                    rng.Seed = Convert.ToUInt64(DateTime.Now.Ticks);
                } else {
                    rng.Seed = Convert.ToUInt64((int)inputs["Seed"].Get());
                }

                rng.Seed += seedAdds;
                seedAdds++;

                var arr = (Godot.Collections.Array)inputs["List"].Get();

                int idx = rng.RandiRange(0, arr.Count - 1);

                if (arr == null) {
                    return null;
                }
                return arr[idx];
            })},
        };
    }

    public override void OnBeforeEvaluation() {
       seedAdds = 0;
    }
}