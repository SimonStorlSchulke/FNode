using System.IO;
using Godot;
using System;

public class FNodeDateCompare : FNode
{
    public FNodeDateCompare() {
        category = "Date";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Date", new FInputDate(this)},
            {"CmpTo", new FInputDate(this)},
            {"CmpTo 2", new FInputDate(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Before", new FOutputBool(this, delegate() 
            {
                int compar = ((DateTime)inputs["Date"].Get()).CompareTo(((DateTime)inputs["CmpTo"].Get()));
                return compar > 0;
            })},
            {
            "After", new FOutputBool(this, delegate() 
            {
                int compar = ((DateTime)inputs["Date"].Get()).CompareTo(((DateTime)inputs["CmpTo"].Get()));
                return compar <= 0;
            })},
            {
            "Difference", new FOutputFloat(this, delegate() 
            {
                TimeSpan ts = ((DateTime)inputs["CmpTo"].Get()) - ((DateTime)inputs["Date"].Get());
                return (float)ts.TotalDays;
            })},
        };
    }
}
