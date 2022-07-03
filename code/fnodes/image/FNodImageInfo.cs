using Godot;
using System;
using ImageMagick;

public class FNodeImageInfo : FNode
{
    MagickImage currentImage;

    public FNodeImageInfo() {
        HintTooltip = "Get information about the connected Image";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Width", new FOutputInt(this, delegate() {
                if (currentImage == null) {
                    return null;
                }
                return currentImage.Width;
            })},
            {
            "Height", new FOutputInt(this, delegate() {
                if (currentImage == null) {
                    return null;
                }
                return currentImage.Height;
            })},
            {
            "Colorspace", new FOutputString(this, delegate() {
                if (currentImage == null) {
                    return null;
                }
                return Enum.GetName(typeof(ColorSpace), currentImage.ColorSpace);
            })},
            {
            "Signature", new FOutputString(this, delegate() {
                if (currentImage == null) {
                    return null;
                }
                foreach (var item in currentImage.AttributeNames)
                {
                    GD.Print(item);
                }
                return "t";
                
            })},
        };
    }
    public override void OnNextIteration() {
        currentImage = inputs["Image"].Get<MagickImage>();
    }
}
