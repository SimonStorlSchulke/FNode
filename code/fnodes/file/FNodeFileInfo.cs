using System.IO;
using Godot;
using System;

public class FNodeFileInfo : FNode {
    // TODO check if File exists
    public FNodeFileInfo() {
        category = "File";
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputFile(this, delegate() {
                return (FileInfo)inputs["File"].Get<object>();
            })},
            {
            "Name", new FOutputString(this, delegate() {
                try {
                    return (System.IO.Path.GetFileNameWithoutExtension(((FileInfo)inputs["File"].Get<object>()).FullName));
                } catch {
                    return "";
                }
            })},
            {
            "Extension", new FOutputString(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).Extension;
                } catch {
                    return "";
                }
            })},{
            "Name with Extension", new FOutputString(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).Name;
                } catch {
                    return "";
                }
            })},
            {
            "Base Dir", new FOutputString(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).Directory.ToString(); //TODO - Add DirectoryInfo Slot Type??
                } catch {
                    return "";
                }
            })},
            {
            "Creation Time", new FOutputDate(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).CreationTime;
                } catch {
                    return null; // Maybe Null here instead?
                }
            })},
            {
            "Last Access Time", new FOutputDate(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).LastAccessTime;
                } catch {
                    return null;
                }
            })},
            {
            "Last Write Time", new FOutputDate(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).LastWriteTime;
                } catch {
                    return null;
                }
            })},
            {
            "Size", new FOutputInt(this, delegate() {
                try {
                    return (int)((FileInfo)inputs["File"].Get<object>()).Length;
                } catch {
                    return 0;
                }
            })},
            {
            "Readonly", new FOutputBool(this, delegate() {
                try {
                    return ((FileInfo)inputs["File"].Get<object>()).IsReadOnly;
                } catch {
                    return true;
                }
            })},
        };
    }
}
