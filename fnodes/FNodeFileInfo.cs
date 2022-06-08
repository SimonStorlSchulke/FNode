using System.IO;
using Godot;
using System;

public class FNodeFileInfo : FNode
{
    public FNodeFileInfo() {

        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this, 0)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Name", new FOutputString(this, 0, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Name;
            })},
            {
            "Extension", new FOutputString(this, 1, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Extension;
            })},
            {
            "Base Dir", new FOutputString(this, 0, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Directory.ToString(); //TODO - Add DirectoryInfo Slot Type??
            })},
            {
            "Creation Time", new FOutputDate(this, 2, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).CreationTime;
            })},
            {
            "Last Access Time", new FOutputDate(this, 3, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).LastAccessTime;
            })},
            {
            "Last Write Time", new FOutputDate(this, 4, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).LastWriteTime;
            })},
            {
            "Size", new FOutputInt(this, 5, delegate() 
            {
                return ((FileInfo)inputs["File"].Get()).Length;
            })},
        };
    }
}
