using System.IO;
using Godot;
using System;

public class FNodeText : FNode
{
    public FNodeText() {
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {
                return inputs["Text"].Get<string>();
            })},
        };
    }
}
