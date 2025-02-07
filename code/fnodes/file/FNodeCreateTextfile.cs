using System.IO;
using Godot;
using System;

public partial class FNodeCreateTextFile : FNode
{
    public FNodeCreateTextFile() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Name", new FInputString(this)},
            {"Path3D", new FInputString(this)},
            {"Toggle", new FInputBool(this, initialValue: true)},
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

        string path = inputs["Path3D"].Get<string>();

        if (!FileUtil.IsAbsolutePath(path)) {
            Errorlog.Log(this, "Filepath is not an absolute Path3D: " + path);
            base.ExecutiveMethod();
            return;
        }

        if (Main.Inst.preview) {
            string p = FileUtil.JoinPaths(path, inputs["Name"].Get<string>());
            PuPreviewOps.AddFileCreated(p);
            base.ExecutiveMethod();
            return;
        }

        FileUtil.CreateDirIfNotExisting(path);

        try {
            string writePath = FileUtil.JoinPaths(path, inputs["Name"].Get<string>());
            System.IO.File.WriteAllText(writePath, inputs["Text"].Get<string>()); // Todo - Replaye with faster Method
        } catch {
            base.ExecutiveMethod();
            return;
        }
        base.ExecutiveMethod();
    }
}
