using System.IO;
using Godot;
using System;

public class FNodeFileInfo : FNode
{
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
            "Name", new FOutputString(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).Name;
                } catch {
                    return "";
                }
            })},
            {
            "Extension", new FOutputString(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).Extension;
                } catch {
                    return "";
                }
            })},
            {
            "Base Dir", new FOutputString(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).Directory.ToString(); //TODO - Add DirectoryInfo Slot Type??
                } catch {
                    return "";
                }
            })},
            {
            "Creation Time", new FOutputDate(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).CreationTime;
                } catch {
                    return new DateTime(); // Maybe Null here instead?
                }
            })},
            {
            "Last Access Time", new FOutputDate(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).LastAccessTime;
                } catch {
                    return new DateTime();;
                }
            })},
            {
            "Last Write Time", new FOutputDate(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).LastWriteTime;
                } catch {
                    return new DateTime();
                }
            })},
            {
            "Size", new FOutputInt(this, delegate() 
            {
                try {
                    return (int)((FileInfo)inputs["File"].Get()).Length;
                } catch {
                    return 0;
                }
            })},
            {
            "Readonly", new FOutputBool(this, delegate() 
            {
                try {
                    return ((FileInfo)inputs["File"].Get()).IsReadOnly;
                } catch {
                    return true;
                }
            })},
        };
    }
}
