using Godot;
using System;
using System.IO;

public class FNodeGetFiles : FNode
{
    FileInfo currentFile;
    public FNodeGetFiles()
    {
        category = "File";
        
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Stack", new FInputInt(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputFile(this, delegate() {
            int stack = (int)inputs["Stack"].Get();
            string path = Main.inst.currentProject.FileStacks.Stacks[stack][Project.idxEval].Item2;
            try {
                if (FileUtil.IsAbsolutePath(path)) return new FileInfo(path);
                else {
                    Errorlog.Log(this, "Only absolute Paths are allowed");
                    return null;
                };
            } catch (System.Exception e) {
                Errorlog.Log(this, e.Message);
                return null;
            }
            })},
            {
            "Name", new FOutputString(this, delegate() {
                try {
                    return System.IO.Path.GetFileNameWithoutExtension(currentFile.Name);
                } catch {
                    return "";
                }
            })},
            {
            "Extension", new FOutputString(this, delegate() {
                return currentFile.Extension;
            })},{
            "Name with Extension", new FOutputString(this, delegate() {
                return currentFile.Name;
            })},
            {
            "Base Dir", new FOutputString(this, delegate() {
                return currentFile.Directory.ToString();
            })},
            {
            "Creation Time", new FOutputDate(this, delegate() {
                return currentFile.CreationTime;
            })},
            {
            "Last Access Time", new FOutputDate(this, delegate() {
                return currentFile.LastAccessTime;
            })},
            {
            "Last Write Time", new FOutputDate(this, delegate() {
                return currentFile.LastWriteTime;
            })},
            {
            "Size", new FOutputInt(this, delegate() {
                return (int)currentFile.Length;
            })},
            {
            "Readonly", new FOutputBool(this, delegate() {
                return currentFile.IsReadOnly;
            })},
        };
    }

    public override void OnNextIteration() {
        string path;
        try {
            path = Main.inst.currentProject.FileStacks.Stacks[(int)inputs["Stack"].Get()][Project.idxEval].Item2;
        } catch(System.Exception e) {
            GD.Print("NANNI1", e);
            currentFile = null;
            return;
        }
        
            try {
                if (FileUtil.IsAbsolutePath(path)) {
                    currentFile = new FileInfo(path);
                }
                else {
                    Errorlog.Log(this, "Only absolute Paths are allowed");
                    GD.Print("NANNI2");
                    currentFile = null;
                    return;
                };
            } catch (System.Exception e) {
                Errorlog.Log(this, e.Message);
                GD.Print("NANNI3");
                currentFile = null;
            }
    }
}