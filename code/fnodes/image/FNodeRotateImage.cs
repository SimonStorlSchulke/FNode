using ImageMagick;

public partial class FNodeRotateImage : FNode
{
    public FNodeRotateImage() {
        TooltipText = "Rotate an Image clockwise by the given degrees (negative values for counterclockwise rotation)";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Degrees", new FInputInt(this)},
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
                    img.BackgroundColor = MagickColors.Transparent;
                    img.Rotate(inputs["Degrees"].Get<int>());
                    return img;
                } catch (MagickException e) {
                    Errorlog.Log(e);
                    return null;
                }
            })},
        };
    }
}
