using System.IO;
using Godot;
using System;

public class FNodeRandomNumber : FNode
{
    ulong seedAdds = 0;
    public FNodeRandomNumber() {
        HintTooltip = "Generate a random numbers";
        category = "Math";
        
        RandomNumberGenerator rng = new RandomNumberGenerator();

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Min", new FInputFloat(this)},
            {"Max", new FInputFloat(this, initialValue: 1f)},
            {"Seed", new FInputInt(this, initialValue: -1, min: -1, description: "If you put -1 here, the seed will be randomized")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Number", new FOutputInt(this, delegate() {
                int uiSeed = inputs["Seed"].Get<int>();
                if (uiSeed == -1) {
                    rng.Seed = Convert.ToUInt64(DateTime.Now.Ticks);
                } else {
                    rng.Seed = Convert.ToUInt64(inputs["Seed"].Get<int>());
                }
                
                rng.Seed += seedAdds;
                seedAdds++;

                if (GetSelectedOption("numbertype") == "Int") {
                    return rng.RandiRange(inputs["Min"].Get<int>(), inputs["Max"].Get<int>());
                } else {
                    return rng.RandfRange(inputs["Min"].Get<float>(), inputs["Max"].Get<float>());
                }
            })},
        };
    }

    public override void OnBeforeEvaluation() {
       seedAdds = 0;
    }


    public void OptionChanged(int idx) {
        if (idx == 1) {
            ChangeSlotType(inputs["Min"], FNodeSlotTypes.INT);
            ChangeSlotType(inputs["Max"], FNodeSlotTypes.INT, initialValue: 100);
            ChangeSlotType(outputs["Number"], FNodeSlotTypes.INT);
        } else if (idx == 0) {
            ChangeSlotType(inputs["Min"], FNodeSlotTypes.FLOAT);
            ChangeSlotType(inputs["Max"], FNodeSlotTypes.FLOAT, initialValue: 1f);
            ChangeSlotType(outputs["Number"], FNodeSlotTypes.FLOAT);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        AddOptionEnum(
            "numbertype",

            new string[] {
                "Float",
                "Int",
            },
            nameof(OptionChanged));
    }
}
