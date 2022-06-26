using Godot;
using System;

public static class GdExtensions
{
    public static void AddChildren(this Node n, params Node[] nodelist) {
        foreach (var node in nodelist) {
            n.AddChild(node);
        }
    }

    public static void Layout(this Control ctl, int minSizeX = 0, int minSizeY=0, bool expandHor = true, bool expandVert = true) {
        if (expandHor) ctl.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        if (expandVert) ctl.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
        ctl.RectMinSize = new Vector2(minSizeX, minSizeY);
    }
}


public class nButton : Button {
    public nButton(string text, Node target, string methodName, Godot.Collections.Array methodParams = null, string name = "") {
        Text = text;
        if (methodParams == null) {
            Connect("pressed", target, methodName);
        } else {
            Connect("pressed", target, methodName, methodParams);
        }
        if (name != "") {
            Name = name;
        }
    }
}
