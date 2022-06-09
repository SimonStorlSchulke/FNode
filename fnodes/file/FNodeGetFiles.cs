using Godot;
using System;
using System.IO;

public class FNodeGetFiles : FNode
{
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
            return new FileInfo(inputs["Path"].Get() as string);
        })},
        };
    }
}
