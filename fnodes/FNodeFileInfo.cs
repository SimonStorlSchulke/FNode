using System.IO;
using Godot;
using System;

public class FNodeFileInfo : FNode
{
    public FNodeFileInfo() {
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Name", new FOutputString(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Name;
            })},
            {
            "Extension", new FOutputString(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Extension;
            })},
            {
            "Base Dir", new FOutputString(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Directory.ToString(); //TODO - Add DirectoryInfo Slot Type??
            })},
            {
            "Creation Time", new FOutputDate(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).CreationTime;
            })},
            {
            "Last Access Time", new FOutputDate(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).LastAccessTime;
            })},
            {
            "Last Write Time", new FOutputDate(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).LastWriteTime;
            })},
            {
            "Size", new FOutputInt(this, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Length;
            })},
        };
    }
}
