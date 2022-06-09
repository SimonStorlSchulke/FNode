using System.IO;
using Godot;
using System;

public class FNodeTextViewer : FNode
{
    public FNodeTextViewer() {
        category = "String";        
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this, 0)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod()
    {
        //TODO Add Exception Handling + create Folder if it doesn't exist.
        GetNode<TextEdit>("Viewer").Text = inputs["Text"].Get() as string;
        base.ExecutiveMethod();
    }

    public override void _Ready()
    {
        base._Ready();
        TextEdit TEViewer = new TextEdit();
        TEViewer.Readonly = true;
        TEViewer.Name = "Viewer";
        TEViewer.RectMinSize = new Vector2(0, 200);
        AddChild(TEViewer);
    }
    
}
