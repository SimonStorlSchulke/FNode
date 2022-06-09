using System.IO;
using Godot;
using System;

public class FNodeMath : FNode
{
    public FNodeMath() {
        HintTooltip = "Returns the Parent Path of the given string.\nIf this fails, it returns the original path instead.";
        category = "Math";        

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Index", new FOutputInt(this, delegate() {
                return Project.idxEval;
            })},
            {
            "Loop Number", new FOutputInt(this, delegate() {
                return Project.maxNumFiles;
            })},
        };
    }
}
