using Godot;
using System;

public class FNodeGetJSONItem: FNode
{
    public FNodeGetJSONItem() {
        HintTooltip = "";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"JSON", new FInput(this)},
            {"Key", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutput(this, delegate() {
                try {
                    return inputs["JSON"].Get<Godot.Collections.Dictionary>()[inputs["Key"].Get<string>()];
                } catch (System.Exception e) {
                    Errorlog.Log(e);
                    return "";
                }
            })},
        };
    }
}
