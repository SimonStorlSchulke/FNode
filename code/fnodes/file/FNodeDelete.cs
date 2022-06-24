using System.IO;
using Godot;
using System;

public class FNodeDelete : FNode
{
    public FNodeDelete() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
            {"Toggle", new FInputBool(this, initialValue: true)},
            //{"Permanently", new FInputBool(this, description: "if this is true, the File will be deleted permanently, else it will be moved to the recycle bin")},
        };
        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod()
    {
        if (!inputs["Toggle"].Get<bool>()) {
            base.ExecutiveMethod();
            return;
        }

        string path = inputs["File"].Get<FileInfo>().FullName;

        FileUtil.SecureDelete(path);
        base.ExecutiveMethod();
    }
}