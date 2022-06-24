using System.IO;
using Godot;
using System;

public class FNodeMoveFile : FNode
{
    public FNodeMoveFile() {
        HintTooltip = "Moves a File to the given folder and creates a Folder if the path doesn't exist";
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
            {"To Path", new FInputString(this)},
            {"Rename?", new FInputString(this, description: "If this is not empty, the File will be renamed (not changing the Extension)")},
            {"Toggle", new FInputBool(this, initialValue: true)},
        };
        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod()
    {
        if (!inputs["Toggle"].Get<bool>()) {
            base.ExecutiveMethod();
            return;
        }
        
        FileInfo fi = inputs["File"].Get<FileInfo>();
        if (fi == null) {
            Errorlog.Log(this, "Illegal Filepath");
            return;
        }
        string newName = inputs["Rename?"].Get<string>();
        string oldpath = fi.FullName;
        string fileName = newName == "" ? System.IO.Path.GetFileNameWithoutExtension(oldpath) : newName;
        string path = inputs["To Path"].Get<string>();
        FileUtil.CreateDirIfNotExisting(path);
        string newPath = FileUtil.JoinPaths(path, fileName + fi.Extension);
        if (newPath == "") {
            return;
        }
        FileUtil.SecureMove(oldpath, newPath);
        fi = new FileInfo(newPath); //TODO  Test this.
        
        base.ExecutiveMethod();
    }
}