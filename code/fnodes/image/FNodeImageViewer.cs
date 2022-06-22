using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeImageViewer : FNode
{
    TextureRect tx;
    public FNodeImageViewer() {
        HintTooltip = "";
        category = "Img";

        Connect("resize_request", this, nameof(OnResize));
        Resizable = true;

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Resolution", new FInputInt(this, initialValue: 256)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
        
    }

    public void OnResize(Vector2 newSize) {
        RectSize = newSize;
    }


    public override void ExecutiveMethod() {
        if (!Project.IsLastIteration) {
            return;
        }
        MagickImage image = (MagickImage)inputs["Image"].Get();
        Image im = ImageUtils.MagickImageToGDImage(image, width: (int)inputs["Resolution"].Get());
        ImageTexture tex = new ImageTexture();
        tex.CreateFromImage(im);

        tx.Texture = tex;
    }

    
    public override void _Ready() {
        base._Ready();
        tx = new TextureRect();
        tx.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        tx.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
        tx.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        tx.RectMinSize = new Vector2(300, 300);
        tx.Expand = true;
        AddChild(tx);
    }
}
