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
            {"To", new FInputString(this)},
        };
        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            /*{"Creation Time", new FOutput(this, 0, delegate() {
                FOutput connTo = inputs["File"].ConnectedTo();
                if (connTo != null) {
                    return System.IO.File.GetCreationTime(connTo.Get<object>() as string);
                } else {
                    return "TODO DEFAULT VALUES";
                }
        })},*/
        };
    }

    public override void ExecutiveMethod()
    {
        string dest = (inputs["File"].Get<FileInfo>()).DirectoryName + "\\" + (inputs["To"].Get<string>());
        try {
            System.IO.File.Move(inputs["File"].Get<FileInfo>().FullName, dest);
        } catch {
            // TODO Exception handling
        }
        base.ExecutiveMethod();
    }
}