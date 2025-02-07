using System.IO;
using Godot;
using System;

public partial class FNodeFilterImages : FNode
{
    public FNodeFilterImages() {
        TooltipText = "Filter Image files by the given Toggle input";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputImage(this)},
            {"Toggle", new FInputBool(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Matching Image", new FOutputImage(this, delegate() 
            {
                if ((bool)inputs["Toggle"].Get<object>()) {
                    return inputs["File"].Get<ImageMagick.MagickImage>();
                }
                else {
                    return null;
                }
            })},
        };
    }
}
