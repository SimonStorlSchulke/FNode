using System.IO;
using Godot;
using System;

public class FNodeCreateTextFile : FNode
{
    public FNodeCreateTextFile() {
        isExecutiveNode = true;

        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this, 0)},
            {"Name", new FInputString(this, 1)},
            {"Path", new FInputString(this, 2)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputFile(this, 0, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Name;
            })},
        };
    }

    public override void ExecutiveMethod()
    {
        string path = System.IO.Path.Combine((string)inputs["Path"].Get(), (string)inputs["Name"].Get());
        System.IO.File.WriteAllText(path, (string)inputs["Text"].Get()); // Todo - Replaye with faster Method
    }
}
