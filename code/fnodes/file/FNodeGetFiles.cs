using Godot;
using System;
using System.IO;

public class FNodeGetFiles : FNode
{
    FileInfo currentFile;
    public FNodeGetFiles()
    {
        category = "File";
        HintTooltip = "For each iteration, return the File at the current Evaluation index";
        
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Stack", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputFile(this, delegate() {
            int stack = (int)inputs["Stack"].Get();
            //string path = Main.inst.currentProject.FileStacks.GetChild<FileList>() Stacks[stack][Project.idxEval].Item2;
            try {
                return currentFile;
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
                try {
                    return currentFile.Extension;
                    }
                catch {
                    return "";
                }}
            )},{
            "Name with Extension", new FOutputString(this, delegate() {
                try {
                    return currentFile.Name;
                    }
                catch {
                    return "";
                }
            })},
            {
            "Base Dir", new FOutputString(this, delegate() {
                try {
                    return currentFile.Directory.ToString();
                    }
                catch {
                    return "";
                }
            })},
            {
            "Creation Time", new FOutputDate(this, delegate() {
                try {
                    return currentFile.CreationTime;
                    }
                catch {
                    return null;
                }
            })},
            {
            "Last Access Time", new FOutputDate(this, delegate() {
                try {
                    return currentFile.LastAccessTime;
                    }
                catch {
                    return null;
                }
            })},
            {
            "Last Write Time", new FOutputDate(this, delegate() {
                try {
                    return currentFile.LastWriteTime;
                    }
                catch {
                    return null;
                }
            })},
            {
            "Size", new FOutputInt(this, delegate() {
                try {
                    return (int)currentFile.Length;
                    }
                catch {
                    return 0;
                }
            })},
            {
            "Readonly", new FOutputBool(this, delegate() {
                try {
                    return currentFile.IsReadOnly;
                    }
                catch {
                    return false;
                }
            })},
        };
    }

    public override void OnNextIteration() {
        string path;
        try {
            path = Main.inst.currentProject.FileStacks.GetChild<FileList>((int)inputs["Stack"].Get()).allFiles[Project.idxEval];
        } catch(System.Exception e) {
            currentFile = null;
            return;
        }
        
            try {
                if (FileUtil.IsAbsolutePath(path)) {
                    currentFile = new FileInfo(path);
                }
                else {
                    Errorlog.Log(this, "Only absolute Paths are allowed");
                    currentFile = null;
                    return;
                };
            } catch (System.Exception e) {
                Errorlog.Log(this, e.Message);
                currentFile = null;
            }
    }

}