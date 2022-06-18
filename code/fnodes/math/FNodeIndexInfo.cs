using System.IO;
using Godot;
using System;

public class FNodeIndexInfo : FNode
{
    public FNodeIndexInfo() {
        HintTooltip = "Returns the Parent Path of the given string.\nIf this fails, it returns the original path instead.";
        category = "Math";        

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Index", new FOutputInt(this, delegate() {
                inputs["File"].Get();
                return Project.idxEval;
            })},
            {
            "Loop Number", new FOutputInt(this, delegate() {
                return (int)Math.Max(Main.inst.currentProject.spIterations.Value, Project.maxNumFiles);
            })},
            {
            "Is last iteration", new FOutputBool(this, delegate() {
                int iterations = (int)Math.Max(Main.inst.currentProject.spIterations.Value, Project.maxNumFiles);
                return Project.idxEval > (iterations-2);
            })},
        };
    }
}
