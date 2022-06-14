using System.IO;
using Godot;
using System;

public class FNodeTextViewer : FNode
{
    string accumulatedString = "";
    public FNodeTextViewer() {
        category = "Text";        
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this, 0)},
            {"Accumulate", new FInputBool(this, 1, description: "If this is set, a new Line will be created for each iteration", initialValue: true)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod() {

        //Doing this here because of process order (instance of main is initialized after Nodes)
        if (!Main.inst.IsConnected(nameof(Main.StartParsing), this, nameof(ResetString))) {
            Main.inst.Connect(nameof(Main.StartParsing), this, nameof(ResetString));
        }

        if ((bool)inputs["Accumulate"].Get()) {
            accumulatedString += inputs["Text"].Get() as string + "\n";
            GetNode<TextEdit>("Viewer").Text = accumulatedString;
        } else {
            GetNode<TextEdit>("Viewer").Text = inputs["Text"].Get() as string;
        }
        base.ExecutiveMethod();
    }

    public override void _Ready() {
        base._Ready();
        TextEdit TEViewer = new TextEdit();
        TEViewer.Readonly = true;
        TEViewer.Name = "Viewer";
        TEViewer.RectMinSize = new Vector2(400, 400);
        AddChild(TEViewer);
    }

    public void ResetString() {
        accumulatedString = "";
    }
    
}
