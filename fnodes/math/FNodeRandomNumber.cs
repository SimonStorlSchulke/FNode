using System.IO;
using Godot;
using System;

public class FNodeRandomNumber : FNode
{
    ulong seedAdds = 0;
    public FNodeRandomNumber() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";
        
        RandomNumberGenerator rng = new RandomNumberGenerator();

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Min", new FInputInt(this)},
            {"Max", new FInputInt(this, initialValue: 100)},
            {"Seed", new FInputInt(this, initialValue: -1, min: -1, description: "If you put -1 here, the seed will be randomized")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Number", new FOutput(this, delegate() {
                int uiSeed = (int)inputs["Seed"].Get();
                if (uiSeed == -1) {
                    rng.Seed = Convert.ToUInt64(DateTime.Now.Ticks);
                } else {
                    rng.Seed = Convert.ToUInt64((int)inputs["Seed"].Get());
                }
                
                rng.Seed += seedAdds;
                seedAdds++;

                if (GetNode<OptionButton>("numbertype").Selected == 0) {
                    return rng.RandiRange((int)inputs["Min"].Get(), (int)inputs["Max"].Get());
                } else {
                    return rng.RandfRange((float)inputs["Min"].Get(), (float)inputs["Max"].Get());
                }
            })},
        };
    }

    public override void OnBeforeEvaluation() {
        GD.Print("AYY");
       seedAdds = 0;
    }


    public void OptionChanged(int idx) {
        if (idx == 0) {
            ChangeSlotType(inputs["Min"], FNodeSlotTypes.INT);
            ChangeSlotType(inputs["Max"], FNodeSlotTypes.INT, initialValue: 100);
            ChangeSlotType(outputs["Number"], FNodeSlotTypes.INT);
        } else if (idx == 1) {
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
                "Int",
                "Float",
            },
            nameof(OptionChanged));
    }
}
