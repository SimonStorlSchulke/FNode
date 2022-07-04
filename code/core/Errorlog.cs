using Godot;
using System;

public class Errorlog : Node
{
    static Color errorColor = new Color(1,.3f,.3f);

    static void WriteLog(string text) {
        string path = FileUtil.JoinPaths(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "FNode/errorlogs");
        FileUtil.CreateDirIfNotExisting(path);
        try {
            string writePath = FileUtil.JoinPaths(path, DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log");
            System.IO.File.WriteAllText(writePath, text); // Todo - Replaye with faster Method
        } catch {
            return;
        }
    }

    public static void Log(FNode thrower, string text) {
        GD.Print($"Error on {thrower.GetType()}: {text}");
        InfoLine.ShowColored($"Error on {thrower.GetType()}: {text}", errorColor);
    }

    public static void Log(string text) {
        GD.Print($"Error: {text}");
        InfoLine.ShowColored($"Error: {text}", errorColor);
    }

    public static void Log(System.Exception e) {
        WriteLog(e.ToString());
        GD.Print($"Error: {e.Message}");
        InfoLine.ShowColored($"Error: {e.Message}", errorColor);
    }

    public static void Log(string str, System.Exception e) {
        GD.Print($"Error: {str} | {e}");
        WriteLog(str + "\n\n" + e.ToString());
        InfoLine.ShowColored($"Error: {str} | {e.Message}", errorColor);
    }

    public static void Log(object thrower, System.Exception e) {
        GD.Print($"Error: {e.Message} on object {thrower.ToString()}");
        WriteLog(thrower.ToString() + "\n\n" + e.ToString());
        InfoLine.ShowColored($"Error: {e.Message} on object {thrower.ToString()}", errorColor);
    }

    public static void Log(FNode thrower, System.Exception e) {
        GD.Print($"Error on {thrower.GetType()}: {e.Message}");
        WriteLog(thrower.GetType() + "\n\n" + e.ToString());
        InfoLine.ShowColored($"Error on {thrower.GetType()}: {e.Message}", errorColor);
    }

}
