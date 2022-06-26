using ImageMagick;

public class FNodeTrimImage : FNode
{
    public FNodeTrimImage() {
        HintTooltip = "Trim the transparent space at the sides of an Image";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Margin", new FInputInt(this, min: 0)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                var img = inputs["Image"].Get<MagickImage>();

                if (img == null) {
                    return null;
                }

                img.Trim();

                int margin = inputs["Margin"].Get<int>();
                if (margin > 0) {
                    img.Extent(img.Width + margin*2, img.Height + margin*2, Gravity.Center);
                    img.RePage();
                }

               return img;
            })},
        };
    }
}
