using Godot;
using System;

public static class GdExtensions
{
    public static void AddChildren(this Node n, params Node[] nodelist) {
        foreach (var node in nodelist) {
            n.AddChild(node);
        }
    }
}


