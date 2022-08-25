using ImageMagick;

public class FNodeMergeImages : FNode
{
    public FNodeMergeImages() {
        HintTooltip = "Composite Image 2 ontop of Image 1. For positional values of 0 or 1, Image 2 will be positioned in the corners.";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image 1", new FInputImage(this)},
            {"Image 2", new FInputImage(this)},
            {"Position X", new FInputFloat(this, initialValue: 0.5f)},
            {"Position Y", new FInputFloat(this, initialValue: 0.5f)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                var img1 = inputs["Image 1"].Get<MagickImage>();
                var img2 = inputs["Image 2"].Get<MagickImage>();

                if (img1 == null) {
                    return null;
                }
                if (img2 == null) {
                    return img1;
                }
                
                try {
                    float px = inputs["Position X"].Get<float>();
                    float py = inputs["Position Y"].Get<float>();
                    
                    img1.Composite(
                        img2,
                        (int)(ImageUtils.MapRange(px, 0, 1, img2.Width / 2, img1.Width - img2.Width / 2) - img2.Width / 2),
                        (int)(ImageUtils.MapRange(py, 0, 1, img2.Height / 2, img1.Height - img2.Height / 2) - img2.Height / 2),
                        CompositeOperator.Atop);
                    return img1;
                } catch (MagickException e) {
                    Errorlog.Log(e);
                    return null;
                }
            })},
        };
    }
}
