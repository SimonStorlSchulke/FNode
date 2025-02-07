using System.IO;
using Godot;
using System;

public partial class FNodeIndexInfo : FNode
{
    public FNodeIndexInfo() {
        TooltipText = "Info about the current iteration index.";
        category = "Math";        

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Index", new FOutputInt(this, delegate() {
                return (int)Project.IdxEval;
            })},
            {
            "Loop Number", new FOutputInt(this, delegate() {
                return (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);
            })},
            {
            "Is last iteration", new FOutputBool(this, delegate() {
                return Project.IsLastIteration;
            })},
        };
    }
}
