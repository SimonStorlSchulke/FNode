using System.IO;
using Godot;
using System;

public partial class FNodeRename : FNode
{
    public FNodeRename() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
            {"To", new FInputString(this)},
            {"Toggle", new FInputBool(this, initialValue: true)},
        };
        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod() {

        if (!inputs["Toggle"].Get<bool>()) {
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