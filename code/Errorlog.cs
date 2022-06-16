using Godot;
using System;

public class Errorlog : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
    }

    public static void Log(FNode thrower, string text) {
        GD.Print($"Error on {thrower.GetType()}:\n{text}");
    }

    public static void Log(string text) {
        GD.Print($"Error:\n{text}");
    }

    public static void Log(System.Exception e) {
        GD.Print($"Error:\n{e.Message}");
    }

    public static void Log(object thrower, System.Exception e) {
        GD.Print($"Error:\n{e.Message} on object {thrower.ToString()}");
    }

}
