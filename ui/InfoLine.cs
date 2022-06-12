using Godot;
using System;

public class InfoLine : Label
{
    public static InfoLine inst;
    Timer clearTimer;

    public override void _Ready() {
        inst = this;
        clearTimer = GetNode<Timer>("ClearTimer");
    }

    public static void Show(string text) {
        inst.clearTimer.Start();
        inst.Text = text;
    }

    public void Clear() {
        inst.Text = "";
    }
}
