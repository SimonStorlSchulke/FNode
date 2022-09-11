using ImageMagick;
using Godot;

public class FNodeCropImage : FNode
{
    public FNodeCropImage() {
        HintTooltip = "Crop Image";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Width", new FInputInt(this, min: 1)},
            {"Height", new FInputInt(this, min: 1)},
            {"Scale to Fit", new FInputBool(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                var img = inputs["Image"].Get<MagickImage>();

                if (img == null) {
                    return null;
                }

                Gravity grav;
                switch (GetSelectedOption("Align")) {
                    case "Center":
                        grav = Gravity.Center;
                        break;
                    case "Top":
                        grav = Gravity.North;
                        break;
                    case "Right":
                        grav = Gravity.East;
                        break;
                    case "Bottom":
                        grav = Gravity.South;
                        break;
                    case "Left":
                        grav = Gravity.West;
                        break;
                    case "Top Left":
                        grav = Gravity.Northwest;
                        break;
                    case "Top Right":
                        grav = Gravity.Northeast;
                        break;
                    case "Bottom Right":
                        grav = Gravity.Southeast;
                        break;
                    case "Bottom Left":
                        grav = Gravity.Southwest;
                        break;
                    default:
                        grav = Gravity.Center;
                        break;
                }

                int width = inputs["Width"].Get<int>();
                int height = inputs["Height"].Get<int>();

                if (inputs["Scale to Fit"].Get<bool>()) {
                    int scaleHeight = height;
                    int scaleWidth = width;

                    float aspect = (float)img.Width / (float)img.Height;
                    if (img.Width > img.Height) {
                        scaleWidth = (int)Mathf.Ceil((width * aspect));
                    } else if (img.Height > img.Width) {
                        scaleHeight = (int)Mathf.Ceil((height * (1 / aspect)));
                    }
                    var size = new MagickGeometry(scaleWidth, scaleHeight);
                    size.Greater = true;
                    img.Resize(size);
                }

                img.Crop(inputs["Width"].Get<int>(), inputs["Height"].Get<int>(), grav);

               return img;
            })},
        };
    }
    
    public override void _Ready() {
        base._Ready();
        AddOptionEnum("Align", new string[] { 
            "Center",
            "Top",
            "Right",
            "Bottom",
            "Left",
            "Top Left",
            "Top Right",
            "Bottom Right",
            "Bottom Left",
        });
    }
}
