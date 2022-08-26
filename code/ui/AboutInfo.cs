using Godot;

[Tool]
public class AboutInfo : Label
{
    [Export] bool update {get{return true;} set {UpdateInfo();}}
    [Export] string Version;

    public void UpdateInfo()
    {
        Text = $"{Version}\nDate: {System.DateTime.Now.ToString("yyyy-MM-dd")}";
    }

}
