using System.IO;
using Godot;
using System;

public class FNodeRename : FNode
{
    public FNodeRename() {
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
                    return System.IO.File.GetCreationTime(connTo.Get() as string);
                } else {
                    return "TODO DEFAULT VALUES";
                }
        })},*/
        };
    }

    public override void ExecutiveMethod() {
        if (inputs["File"].Get() == null) {
            base.ExecutiveMethod();
            return;
        }
        string dest = ((FileInfo)inputs["File"].Get()).DirectoryName + "\\" + ((string)inputs["To"].Get());
        try {
            System.IO.File.Move(((FileInfo)inputs["File"].Get()).FullName, dest);
        } catch {
            // TODO Exception handling
        }
        base.ExecutiveMethod();
    }
}