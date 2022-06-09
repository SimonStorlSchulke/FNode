using Godot;
using System;

public class FNodeRename : FNode
{
    public FNodeRename() {

        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputString(this, 0)},
            {"To", new FInputString(this, 1)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            /*{"Creation Time", new FOutput(this, 0, delegate() {
                FOutput connTo = inputs["File"].ConnectedTo();
                if (connTo != null) {
                    return System.IO.File.GetCreationTime(connTo.Get() as string);
                } else {
                    return "TODO DEFAULT VALUES";
                }
        })},*/
        };
    }
}