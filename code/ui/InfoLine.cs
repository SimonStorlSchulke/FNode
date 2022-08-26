using Godot;

///<summary>Displays text for a short time</summary>
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
        inst.AddColorOverride("font_color", Colors.White);
        inst.Text = text;
    }

    public static void ShowColored(string text, Color color) {
        inst.clearTimer.Start();
        inst.AddColorOverride("font_color", color);
        inst.Text = text;
    }

    public void Clear() {
        inst.Text = "";
    }
}
