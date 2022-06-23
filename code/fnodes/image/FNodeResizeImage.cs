using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeResizeImage : FNode
{
    public FNodeResizeImage() {
        HintTooltip = "Multiple Math Operations";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Img", new FInputImage(this)},
            {"Width", new FInputInt(this)},
            {"Height", new FInputInt(this)},
            {"Downscale Only", new FInputBool(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                                    
                var img = inputs["Img"].Get<MagickImage>();
                int width = inputs["Width"].Get<int>();

                switch (GetSelectedOption("Mode")) {
                    case "Keep Aspect Width":
                    //if (geometry.Width > maxWidth) {
                    //    img.Resize(maxWidth, maxHeight);
                    //}
                        break;
                    case "Keep Aspect Height":
                        break;
                    case "Squish":
                        break;
                }

                try {
                    //img.res
                    return new MagickImage(inputs["File"].Get<FileInfo>());
                } catch {
                    return null;
                }
            })},
        };
    }

    public override void _Ready() {
        base._Ready();
        AddOptionEnum("Mode", new string[] {
            "Keep Aspect Width",
            "Keep Aspect Height",
            "Squish",
        },
        description: "'Keep Aspect Width' uses the width Value and sets the Height accurding to the Aspect Ratio.\n'Keep Aspect Height' uses the Height Value\nSquish uses both values and ignores the Aspect Ratio");
    }
}
