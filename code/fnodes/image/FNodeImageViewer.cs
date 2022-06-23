using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeImageViewer : FNode
{
    TextureButton tx;
    AcceptDialog wd;
    ImageTexture tex;


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
        MagickImage image = inputs["Image"].Get<MagickImage>();
        Image im = ImageUtils.MagickImageToGDImage(image, width: inputs["Resolution"].Get<int>());
        tex = new ImageTexture();
        tex.CreateFromImage(im);

        tx.TextureNormal = tex;
    }

    
    public override void _Ready() {
        base._Ready();
        tx = new TextureButton();
        tx.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        tx.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
        tx.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
        tx.RectMinSize = new Vector2(300, 300);
        tx.Expand = true;
        AddChild(tx);
        tx.Connect("pressed", this, nameof(Enlarge));
        wd = new AcceptDialog();
        AddChild(wd);

        wd.Resizable = true;
        TextureRect tr = new TextureRect();
        tr.Name = "TR";
        tr.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        tr.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
        tr.Expand = true;
        tr.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        wd.AddChild(tr);
    }

    public void Enlarge() {
        wd.RectMinSize = GetTree().Root.Size;
        var tr = wd.GetNode<TextureRect>("TR");
        tr.Texture = tex;
        wd.PopupCentered();
    }
}
