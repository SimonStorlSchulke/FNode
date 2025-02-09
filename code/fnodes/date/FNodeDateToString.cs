using System.IO;
using Godot;
using System;

public partial class FNodeDateToString : FNode
{
    public FNodeDateToString() {
        TooltipText = "Converts a Date to a string.\nFor Formating Examples, see docs.microsoft.com/de-de/dotnet/api/system.datetime.tostring"; //TODO better doc
        category = "Date";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Date", new FInputDate(this)},
            {"Format", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate()  {
                try {
                    return inputs["Date"].Get<DateTime>().ToString(inputs["Format"].Get<string>());
                } catch {
                    Errorlog.Log(this, "No Date was given");
                    return "";
                }
            })},
        };
    }
}
