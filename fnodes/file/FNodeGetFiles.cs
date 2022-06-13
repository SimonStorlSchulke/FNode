using Godot;
using System;
using System.IO;

public class FNodeGetFiles : FNode {
    public FNodeGetFiles() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path", new FInputString(this)},
            {"Include Subfolders", new FInputBool(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"File", new FOutputFile(this, delegate() {
                string path = inputs["Path"].Get() as string;
                try {
                    if (FileUtil.IsAbsolutePath(path)) return new FileInfo(path);
                    else {
                        Errorlog.Log(this, "Only absolute Paths are allowed"); 
                        return null;
                    }
                }
                catch (System.Exception e) {
                    Errorlog.Log(this, e.Message);
                    return null;
            }
        })},
        };
    }
}
