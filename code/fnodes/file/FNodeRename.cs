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

        if (!(bool)inputs["Filter"].Get<object>()) {
            base.ExecutiveMethod();
            return;
        }

        if (inputs["File"].Get<object>() == null) {
            base.ExecutiveMethod();
            return;
        }

        FileUtil.SecureRename(((FileInfo)inputs["File"].Get<object>()).FullName, (string)inputs["To"].Get<object>());

        base.ExecutiveMethod();
    }
}