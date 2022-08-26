using Godot;

public class FProgressBar : Godot.ProgressBar
{
    public static FProgressBar inst;

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
