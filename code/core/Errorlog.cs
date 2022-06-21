using Godot;
using System;

public class Errorlog : Node
{
    static Color errorColor = new Color(1,.3f,.3f);

    public static void Log(FNode thrower, string text) {
        GD.Print($"Error on {thrower.GetType()}: {text}");
        InfoLine.ShowColored($"Error on {thrower.GetType()}: {text}", errorColor);
    }

    public static void Log(string text) {
        GD.Print($"Error: {text}");
        InfoLine.ShowColored($"Error: {text}", errorColor);
    }

    public static void Log(System.Exception e) {
        GD.Print($"Error: {e.Message}");
        InfoLine.ShowColored($"Error: {e.Message}", errorColor);
    }

    public static void Log(string str, System.Exception e) {
        GD.Print($"Error: {str} | {e}");
        InfoLine.ShowColored($"Error: {str} | {e.Message}", errorColor);
    }

    public static void Log(object thrower, System.Exception e) {
        GD.Print($"Error: {e.Message} on object {thrower.ToString()}");
        InfoLine.ShowColored($"Error: {e.Message} on object {thrower.ToString()}", errorColor);
    }

    public static void Log(FNode thrower, System.Exception e) {
        GD.Print($"Error on {thrower.GetType()}: {e.Message}");
        InfoLine.ShowColored($"Error on {thrower.GetType()}: {e.Message}", errorColor);
    }

}
