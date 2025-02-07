using Godot;
using ImageMagick;

public partial class FNodeImageViewer : FNode
{
    TextureButton tx;
    ImageTexture tex;
    MagickImage image;


    public FNodeImageViewer() {
        TooltipText = "Displays the connected Image on the Node itself when running the NodeTree. Click to enlarge.";
        category = "Img";

        Connect("resize_request", new Callable(this, nameof(OnResize)));
        Resizable = true;


        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
        
    }

    public void OnResize(Vector2 newSize) {
        Size = newSize;
    }


    public override void ExecutiveMethod() {
        if (!Project.IsLastIteration) {
            return;
        }
        image = inputs["Image"].Get<MagickImage>();
        if (image == null) {
            tx.TextureNormal = null;
            return;
        }
        Image im = ImageUtils.MagickImageToGDImage(image);
        tex = ImageTexture.CreateFromImage(im);

        tx.TextureNormal = tex;
    }

    
    public override void _Ready() {
        base._Ready();
        tx = new TextureButton();
        tx.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        tx.SizeFlagsVertical = SizeFlags.ExpandFill;
        tx.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
        tx.CustomMinimumSize = new Vector2(300, 300);
        //tx.Expand = true;
        AddChild(tx);
        Label lbl = new Label();
        lbl.Text = "Click to enlarge";
        AddChild(lbl);
        tx.Connect("pressed", new Callable(this, nameof(Enlarge)));
    }

    public void Enlarge() {
        if (image == null) {
            return;
        }
        ImageViewerFullscreen.inst.SetImage(tex, image);
        ImageViewerFullscreen.inst.ShowViewer();
    }
}
