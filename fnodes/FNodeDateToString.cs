using System.IO;
using Godot;
using System;

public class FNodeDateToString : FNode
{
    public FNodeDateToString() {

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Date", new FInputDate(this)},
            {"Format", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "String", new FOutputString(this, delegate() 
            {
                return ((DateTime)inputs["Date"].Get()).ToString(inputs["Format"].Get() as string);
            })},
        };
    }
}
