using Godot;

public class ImageViewerFullscreen : Panel
{
    [Export] NodePath NPViewer;
    ImageViewer viewer;
    [Export] NodePath NPLblRes;
    Label lblRes;
    [Export] NodePath NPLblDepth;
    Label lblDeph;
    public static ImageViewerFullscreen inst;
    public override void _Ready() {
        inst = this;
        viewer = GetNode<ImageViewer>(NPViewer);
        viewer.Texture = new ImageTexture();
        lblRes = GetNode<Label>(NPLblRes);
        lblDeph = GetNode<Label>(NPLblDepth);
    }

    public void SetImage(Image img) {
        ((ImageTexture)viewer.Texture).CreateFromImage(img);
    }

    public void SetImage(ImageTexture img) {
        viewer.Texture = img;
    }

    public void SetImage(ImageTexture img, ImageMagick.MagickImage dataReferenceImage) {
        lblRes.Text = $"x: {dataReferenceImage.Width} y: {dataReferenceImage.Height}";
        lblDeph.Text = dataReferenceImage.Depth.ToString();;
        viewer.Texture = img;
    }

    public override void _Input(InputEvent e) {
        if (e.IsActionPressed("ui_cancel")) {
            Close();
        }
    }

    public void Close() {
        Visible = false;
    }

    public void ShowViewer() {
        viewer.RectScale = new Vector2(1, 1);
        viewer.RectSize = OS.WindowSize * 0.8f;
        viewer.RectPosition = RectSize / 2 - viewer.RectSize / 2;
        Show();
    }
}
