using Godot;
using ImageMagick;

public class FNodeAddBackground : FNode
{
    public FNodeAddBackground() {
        HintTooltip = "Remplace transparent parts of the Image with a color";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Color", new FInputColor(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                var img = inputs["Image"].Get<MagickImage>();

                if (img == null) {
                    return null;
                }
                
                try {
                    var c = inputs["Color"].Get<Color>();
                    img.BackgroundColor = c.ToMagickColor();;
                    img.Alpha(AlphaOption.Remove);
                    GD.Print(c.ToMagickColor());
                    return img;
                } catch (MagickException e) {
                    Errorlog.Log(e);
                    return null;
                }
            })},
        };
    }
}
