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
            {"Filter", new FInputBool(this, initialValue: true)},
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
        
        if (!(bool)inputs["Filter"].Get()) {
            base.ExecutiveMethod();
            return;
        }

        if (inputs["File"].Get() == null) {
            base.ExecutiveMethod();
            return;
        }

        FileUtil.SecureRename(((FileInfo)inputs["File"].Get()).FullName, (string)inputs["To"].Get());

        base.ExecutiveMethod();
    }
}