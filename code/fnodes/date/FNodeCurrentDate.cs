using System.IO;
using Godot;
using System;

public partial class FNodeCurrentDate : FNode
{
    public FNodeCurrentDate() {
        category = "Date";

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Date", new FOutputDate(this, delegate() 
            {
                return DateTime.Now;
            })},
        };
    }
}
