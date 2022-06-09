using Godot;
using System;
using System.IO;

public class FNodeGetFilesFromStack : FNode
{
    public FNodeGetFilesFromStack() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Stack", new FInputInt(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"Files", new FOutputFile(this, delegate() {
            int stack = (int)inputs["Stack"].Get();
            return new FileInfo(Main.inst.currentProject.FileStacks.Stacks[stack][Project.idxEval].Item2);
        })},
        };
    }
}
