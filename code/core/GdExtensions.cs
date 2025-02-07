using Godot;
using System;

public static class GdExtensions
{
    public static void AddChildren(this Node n, params Node[] nodelist) {
        foreach (var node in nodelist) {
            n.AddChild(node);
        }
    }

    ///<summary>Shorthand to set common UI layout parameters</summary>
    public static void Layout(this Control ctl, int minSizeX = 0, int minSizeY=0, bool expandHor = true, bool expandVert = true) {
        if (expandHor) ctl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        if (expandVert) ctl.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        ctl.CustomMinimumSize = new Vector2(minSizeX, minSizeY);
    }
}


///<summary>Shorthand for creating Buttons</summary>
public partial class nButton : Button {
    public nButton(string text, Node target, Action method, Godot.Collections.Array methodParams = null, string name = "") {
        Text = text;
        if (methodParams == null) {
            Connect(Button.SignalName.Pressed, Callable.From(method)); //TODO migration - might not work
        } else {
            Connect(Button.SignalName.Pressed, Callable.From(method));
        }
        if (name != "") {
            Name = name;
        }
    }
}
