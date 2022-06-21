using System.IO;
using Godot;
using System;

public class FNodeTextViewer : FNode
{
    string accumulatedString = "";
    public FNodeTextViewer() {

        Resizable = true;
        Connect("resize_request", this, nameof(OnResize));

        category = "Text";        
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this, 0)},
            {"Accumulate", new FInputBool(this, 1, description: "If this is set, a new Line will be created for each iteration", initialValue: true)},
        };

        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public void OnResize(Vector2 newSize) {
        RectSize = newSize;
    }

    public override void ExecutiveMethod() {

        if ((bool)inputs["Accumulate"].Get()) {
            string text = inputs["Text"].Get() as string;
            if (text != "" && text != null) {
                accumulatedString += text + "\n";
            }
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
        TEViewer.RectMinSize = new Vector2(350, 300);
        TEViewer.SizeFlagsHorizontal = TEViewer.SizeFlagsVertical =  (int)SizeFlags.ExpandFill;
        AddChild(TEViewer);
    }

    public override void OnBeforeEvaluation() {
        accumulatedString = "";
    }
}
