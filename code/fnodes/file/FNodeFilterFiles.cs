using System.IO;
using Godot;
using System;

public class FNodeFilterFiles : FNode
{
    public FNodeFilterFiles() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
            {"Filter", new FInputBool(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Matching File", new FOutputFile(this, delegate() 
            {
                if ((bool)inputs["Filter"].Get<object>()) {
                    return inputs["File"].Get<FileInfo>();
                }
                else {
                    return null;
                }
            })},
        };
    }
}
