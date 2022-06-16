using System.IO;
using Godot;
using System;

public class FNodeCreateTextFile : FNode
{
    public FNodeCreateTextFile() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Name", new FInputString(this)},
            {"Path", new FInputString(this)},
            {"Filter", new FInputBool(this, initialValue: true)},
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
        if (!(bool)inputs["Filter"].Get()) {
            base.ExecutiveMethod();
            return;
        }
        if (GetParent<NodeTree>().previewMode) {
            string p = FileUtil.JoinPaths((string)inputs["Path"].Get(), (string)inputs["Name"].Get());
            PuPreviewOps.AddFileCreated(p);
            base.ExecutiveMethod();
            return;
        }

        FileUtil.CreateDirIfNotExisting((string)inputs["Path"].Get());

        try {
            string path = FileUtil.JoinPaths((string)inputs["Path"].Get(), (string)inputs["Name"].Get());
            System.IO.File.WriteAllText(path, (string)inputs["Text"].Get()); // Todo - Replaye with faster Method
        } catch {
            base.ExecutiveMethod();
            return;

        }
        base.ExecutiveMethod();
    }
}
