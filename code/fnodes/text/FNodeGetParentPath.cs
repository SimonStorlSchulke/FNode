using System.IO;
using Godot;
using System;

public class FNodeGetParentPath : FNode
{
    public FNodeGetParentPath() {
        HintTooltip = "Returns the Parent Path of the given string.\nIf this fails, it returns the original path instead.";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Path", new FOutputString(this, delegate() 
            {
                string path = inputs["Path"].Get() as string;
                string parentPath;
                try {
                    parentPath = System.IO.Directory.GetParent(path).FullName;
                } catch {
                    parentPath = null;
                }
                if (parentPath != null)
                    path = parentPath;
                return path;
            })},
        };
    }
}
