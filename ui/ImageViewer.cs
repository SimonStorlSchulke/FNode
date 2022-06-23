using Godot;
using System;

public class ImageViewer : TextureRect
{
    Control viewSpace;

    public override void _Ready() {
        viewSpace = GetParentControl();
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouseButton) {
            if (((InputEventMouseButton)e).ButtonIndex == (int)ButtonList.WheelUp) {
                Zoom(1.15f);
            }
            else if (((InputEventMouseButton)e).ButtonIndex == (int)ButtonList.WheelDown) {
                Zoom(0.85f);
            }
            else if (((InputEventMouseButton)e).ButtonIndex == (int)ButtonList.Left) {
                mouseStartPos = GetGlobalMousePosition();
                ViewerStartPos = RectPosition;
                draggingView = !draggingView;
            }
        }
    }

    public void Zoom(float factor) {
        Vector2 mousePosGloabal = viewSpace.GetGlobalMousePosition();
        RectScale *= factor;
        RectPosition = mousePosGloabal - (new Vector2(1920, 1080) * RectScale) / 2 + (mousePosGloabal - RectPosition) * factor;
    }

    public bool draggingView;
    public override void _Process(float delta) {
        if (draggingView)
            DragView();
    }

    public Vector2 mouseStartPos;
    public Vector2 ViewerStartPos;
    void DragView() {
        RectPosition = ViewerStartPos + GetGlobalMousePosition() - mouseStartPos;
    }
}
