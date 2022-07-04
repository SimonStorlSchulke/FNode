using Godot;
using System;

public class FProgressBar : Godot.ProgressBar
{
    public static FProgressBar inst;
    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree() {
        inst = this;
    }

    public void StartProgress() {
        Visible = true;
    }

    public void ShowProgress(int percent) {
        Value = percent;
    }

    public void ShowProgress(float zeroToOne) {
        Value = zeroToOne * 100;
    }

    public void EndProgress() {
        Value = 0;
        Visible = false;
    }
}
