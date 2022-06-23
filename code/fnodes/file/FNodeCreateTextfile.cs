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
                return ((FileInfo)inputs["File"].Get<object>()).Name;
            })},
        };
    }

    public override void ExecutiveMethod()
    {
        if (!(bool)inputs["Filter"].Get<object>()) {
            base.ExecutiveMethod();
            return;
        }
        if (GetParent<NodeTree>().previewMode) {
            string p = FileUtil.JoinPaths((string)inputs["Path"].Get<object>(), (string)inputs["Name"].Get<object>());
            PuPreviewOps.AddFileCreated(p);
            base.ExecutiveMethod();
            return;
        }

        FileUtil.CreateDirIfNotExisting((string)inputs["Path"].Get<object>());

        try {
            string path = FileUtil.JoinPaths((string)inputs["Path"].Get<object>(), (string)inputs["Name"].Get<object>());
            System.IO.File.WriteAllText(path, (string)inputs["Text"].Get<object>()); // Todo - Replaye with faster Method
        } catch {
            base.ExecutiveMethod();
            return;

        }
        base.ExecutiveMethod();
    }
}
