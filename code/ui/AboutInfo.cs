using Godot;
using System;

[Tool]
public class AboutInfo : Label
{
    [Export] bool update {get{return true;} set {UpdateInfo();}}
    [Export] string Version;
    // Called when the node enters the scene tree for the first time.
    public void UpdateInfo()
    {
        Text = $"{Version}\nDate: {System.DateTime.Now.ToString("yyyy-MM-dd")}";
    }

}
