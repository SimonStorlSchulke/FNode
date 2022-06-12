using Godot;
using System;

public static class FileUtil
{
    public static void CreateDirIfNotExisting(string path) {
        if(!System.IO.Directory.Exists(path) && IsAbsolutePath(path))
        try {
            System.IO.Directory.CreateDirectory(path);
        } catch (System.Exception e) {
            Errorlog.Log(e);
        }
    }

    public static void SecureMove(string fromPath, string toPath) {
        if (!IsAbsolutePath(toPath) || !IsAbsolutePath(fromPath)) {
            Errorlog.Log($"Cannot Move {fromPath.GetFile()} to {toPath} - only absolute Paths are allowed.");
            return;
        }
        CreateDirIfNotExisting(toPath.GetBaseDir());
        try {
            System.IO.File.Move(fromPath, toPath);
            GD.Print($"Moving {fromPath} to {toPath}");
        } catch (System.Exception e) {
            Errorlog.Log(e);
        }
    }

    public static string JoinPaths(string path1, string path2) {
        try {
            return System.IO.Path.Combine(path1, path2);
        } catch (Exception e) {
            Errorlog.Log(e);
            return "";
        }
    }

    public static bool IsAbsolutePath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length < 2) return false; //There is no way to specify a fixed path with one character (or less).
            if (path.Length == 2 && IsValidDriveChar(path[0]) && path[1] == System.IO.Path.VolumeSeparatorChar) return true; //Drive Root C:
            if (path.Length >= 3 && IsValidDriveChar(path[0]) &&  path[1] == System.IO.Path.VolumeSeparatorChar && IsDirectorySeperator(path[2])) return true; //Check for standard paths. C:\
            if (path.Length >= 3 && IsDirectorySeperator(path[0]) && IsDirectorySeperator(path[1])) return true; //This is start of a UNC path
            return false; //Default
        }

        private static bool IsDirectorySeperator(char c) => c == System.IO.Path.DirectorySeparatorChar | c == System.IO.Path.AltDirectorySeparatorChar;
        private static bool IsValidDriveChar(char c) => c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';

}
