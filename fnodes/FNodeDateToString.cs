using System.IO;
using Godot;
using System;

public class FNodeDateToString : FNode
{
    public FNodeDateToString() {

        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Date", new FInputDate(this, 0)},
            {"Format", new FInputString(this, 1)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "String", new FOutputString(this, 0, delegate() 
            {
                return ((DateTime)inputs["Date"].Get()).ToString(inputs["Format"].Get() as string);
            })},
        };
    }
}
