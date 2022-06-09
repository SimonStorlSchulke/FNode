using System.IO;
using Godot;
using System;

public class FNodeCreateTextFile : FNode
{
    public FNodeCreateTextFile() {

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Name", new FInputString(this)},
            {"Path", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputFile(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Name;
            })},
        };
    }

    public override void ExecutiveMethod()
    {
        //TODO Add Exception Handling + create Folder if it doesn't exist.
        string path = System.IO.Path.Combine((string)inputs["Path"].Get(), (string)inputs["Name"].Get());
        System.IO.File.WriteAllText(path, (string)inputs["Text"].Get()); // Todo - Replaye with faster Method
        base.ExecutiveMethod();
    }
}
