using System.IO;
using Godot;
using System;

public partial class FNodeDateCompare : FNode
{
    public FNodeDateCompare()
    {
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
            {   try {
                int compar = inputs["Date"].Get<DateTime>().CompareTo(inputs["CmpTo"].Get<DateTime>());
                return compar > 0;
            } catch(System.Exception e) {
                return 0;
            }
            })},
            {
            "After", new FOutputBool(this, delegate()
            {   try {

                int compar = inputs["Date"].Get<DateTime>().CompareTo(inputs["CmpTo"].Get<DateTime>());
                return compar <= 0;
            } catch(System.Exception e) {
                return 0;
            }
            })},
            {
            "Difference", new FOutputFloat(this, delegate()
            {   try {

                TimeSpan ts = inputs["CmpTo"].Get<DateTime>() - inputs["Date"].Get<DateTime>();
                return (float)ts.TotalDays;
            } catch(System.Exception e) {
                return 0;
            }
            })},
        };
    }
}
