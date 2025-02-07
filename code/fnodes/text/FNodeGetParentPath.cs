using Godot;
using System;

public partial class FNodeGetParentPath : FNode
{
    public FNodeGetParentPath() {
        TooltipText = "Returns the Parent Path3D of the given string.\nIf this fails, it returns the original path instead.";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path3D", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Path3D", new FOutputString(this, delegate() 
            {
                string path = inputs["Path3D"].Get<string>();
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
