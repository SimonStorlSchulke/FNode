using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeConvertImage : FNode
{
    public FNodeConvertImage() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Output Path", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutputFloat(this, delegate() {

                return 0f;
            })},
        };
    }

    public override void ExecutiveMethod() {

        using (MagickImage image = new MagickImage((string)inputs["Image"].Get())) {
             image.Write(((string)inputs["Output Path"].Get()));
        }
        base.ExecutiveMethod();

    }
}
