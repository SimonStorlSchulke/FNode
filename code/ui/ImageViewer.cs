using Godot;

public partial class ImageViewer : TextureRect
{
    Control viewSpace;

    public override void _Ready() {
        viewSpace = GetParentControl();
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouseButton) {
            if (((InputEventMouseButton)e).ButtonIndex == MouseButton.WheelUp) {
                Zoom(1.15f);
            }
            else if (((InputEventMouseButton)e).ButtonIndex == MouseButton.WheelDown) {
                Zoom(0.85f);
            }
            else if (((InputEventMouseButton)e).ButtonIndex == MouseButton.Left) {
                mouseStartPos = GetGlobalMousePosition();
                ViewerStartPos = Position;
                draggingView = !draggingView;
            }
        }
    }

    public void Zoom(float factor) {
        Vector2 mousePosGloabal = viewSpace.GetGlobalMousePosition();
        Scale *= factor;
        Position = mousePosGloabal - (new Vector2(1920, 1080) * Scale) / 2 + (mousePosGloabal - Position) * factor;
    }

    public bool draggingView;
    public override void _Process(double delta) {
        if (draggingView)
            DragView();
    }

    public Vector2 mouseStartPos;
    public Vector2 ViewerStartPos;
    void DragView() {
        Position = ViewerStartPos + GetGlobalMousePosition() - mouseStartPos;
    }
}
