using System.IO;
using Godot;
using System;



public partial class FNodeValueSwitch : FNode
{
    OptionButton ob;

    public FNodeValueSwitch() {
        TooltipText = "Switch between the values of the False and True inputs depending on the state of the Switch input.";
        category = "Other";


        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Switch", new FInputBool(this)},
            {"False", new FInput(this)},
            {"True", new FInput(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"Result", 
            new FOutput(this, delegate() {
                object valFalse = inputs["False"].Get<object>();
                object valTrue = inputs["True"].Get<object>();
                return (object)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
            })},
        };
    }
}
