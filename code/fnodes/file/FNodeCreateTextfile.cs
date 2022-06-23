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
                return inputs["File"].Get<FileInfo>().Name;
            })},
        };
    }

    public override void ExecutiveMethod()
    {
        if (!inputs["Filter"].Get<bool>()) {
            base.ExecutiveMethod();
            return;
        }
        if (GetParent<NodeTree>().previewMode) {
            string p = FileUtil.JoinPaths(inputs["Path"].Get<string>(), inputs["Name"].Get<string>());
            PuPreviewOps.AddFileCreated(p);
            base.ExecutiveMethod();
            return;
        }

        FileUtil.CreateDirIfNotExisting((string)inputs["Path"].Get<string>());

        try {
            string path = FileUtil.JoinPaths(inputs["Path"].Get<string>(), inputs["Name"].Get<string>());
            System.IO.File.WriteAllText(path, inputs["Text"].Get<string>()); // Todo - Replaye with faster Method
        } catch {
            base.ExecutiveMethod();
            return;

        }
        base.ExecutiveMethod();
    }
}
