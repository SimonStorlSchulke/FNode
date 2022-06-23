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
        };
    }

    public override void ExecutiveMethod() {

        if (!inputs["Filter"].Get<bool>()) {
            base.ExecutiveMethod();
            return;
        }

        if (inputs["File"].Get<FileInfo>() == null) {
            base.ExecutiveMethod();
            return;
        }

        FileUtil.SecureRename(inputs["File"].Get<FileInfo>().FullName, inputs["To"].Get<string>());

        base.ExecutiveMethod();
    }
}