using System.IO;
using Godot;
using System;

public class FNodeCopy : FNode
{
    public FNodeCopy() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
            {"To Path", new FInputString(this)},
            {"Name", new FInputString(this)},
        };
        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {

        };
    }

    public override void ExecutiveMethod()
    {
        string dest = System.IO.Path.Combine((string)inputs["To Path"].Get(), (string)inputs["Name"].Get());
        try {
            System.IO.File.Copy(((FileInfo)inputs["File"].Get()).FullName, dest);
        } catch {
            // TODO Exception handling
        }
        base.ExecutiveMethod();
    }
}