using Godot;
using System;
using System.Collections.Generic;

public partial class FNodePickRandom : FNode
{
    ulong seedAdds = 0;

    public FNodePickRandom() {
        TooltipText = "Retrieve a random Item from a List. The output is of a variable Type.";
        category = "List";

        RandomNumberGenerator rng = new RandomNumberGenerator();

        IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
            {"Seed", new FInputInt(this, initialValue: -1, min: -1, description: "If you put -1 here, the seed will be randomized")},
        };

        IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Item", new FOutput(this, delegate() {

                int uiSeed = inputs["Seed"].Get<int>();
                if (uiSeed == -1) {
                    rng.Seed = Convert.ToUInt64(DateTime.Now.Ticks);
                } else {
                    rng.Seed = Convert.ToUInt64(inputs["Seed"].Get<int>());
                }

                rng.Seed += seedAdds;
                seedAdds++;

                var arr = inputs["List"].Get<List<object>>();

                int idx = rng.RandiRange(0, arr.Count - 1);

                if (arr == null) {
                    return null;
                }
                return arr.Count > 0 ? arr[idx] : null;
            })},
        };
    }

    public override void OnBeforeEvaluation() {
       seedAdds = 0;
    }
}
