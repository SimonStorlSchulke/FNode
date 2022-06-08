using Godot;
using System;
using System.IO;

public class FNodeGetFiles : FNode
{
    public FNodeGetFiles() {
        
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Path", new FInputString(this, 0)},
            {"Include Subfolders", new FInputBool(this, 1)},
        };


        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"File", new FOutputFile(this, 0, delegate() {
            return new FileInfo(inputs["Path"].Get() as string);
        })},
        };
    }
}
