using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeGetImage : FNode
{
    public FNodeGetImage() {
        HintTooltip = "Multiple Math Operations";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                try {
                    return new MagickImage((FileInfo)inputs["File"].Get());
                } catch {
                    return null;
                }
            })},
        };
    }
}
