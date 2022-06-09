using Godot;
using System;

public class main : VBoxContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //GetTree().Root.Connect("size_changed", this, nameof(OnSizeChanged));
    }

    public void OnSizeChanged() {
        float scaleMul = 0.5f;
        Vector2 winSize = OS.WindowSize;
        float scale = Mathf.Min(winSize.x / 1920f, winSize.y / 1080f);
        scale /= scaleMul;
        GD.PrintT(winSize);
        RectScale = new Vector2(1f * (1f / scale), 1f * (1f / scale));
        GD.PrintT(RectScale);
        RectSize = winSize * RectScale * (16f / 9f);// / RectScale;//(2f * RectScale);// * RectScale;
    }

}
