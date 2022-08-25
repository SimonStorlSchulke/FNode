using ImageMagick;

public class FNodeResizeImage : FNode
{
    public FNodeResizeImage() {
        HintTooltip = "Resize an Image to a given size";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Width", new FInputInt(this, min: 0, initialValue: 512)},
            {"Height", new FInputInt(this, min: 0, initialValue: 512)},
            {"Downscale Only", new FInputBool(this, initialValue: true)},
            {"Ignore Aspect", new FInputBool(this, initialValue: false)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                var img = inputs["Image"].Get<MagickImage>();

                if (img == null) {
                    return null;
                }

                int width = inputs["Width"].Get<int>();
                int height = inputs["Height"].Get<int>();

                if (GetSelectedOption("Mode") == "Percent") {
                    width = (int)(img.Width * ((float)width / 100));
                    height = (int)(img.Height * ((float)height / 100));
                }

                var size = new MagickGeometry(width, height);

                size.Greater = inputs["Downscale Only"].Get<bool>();
                size.IgnoreAspectRatio = inputs["Ignore Aspect"].Get<bool>();
                
                try {
                    img.Resize(size);
                    return img;
                } catch (MagickException e) {
                    Errorlog.Log(e);
                    return null;
                }
            })},
        };
    }

    public override void _Ready() {
        base._Ready();
        AddOptionEnum("Mode", new string[] { 
            "Pixel",
            "Percent"
        });
    }
}
