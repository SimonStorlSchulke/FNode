using Godot;
using System;
using System.Collections.Generic;

public static class DevUtil
{
    public static string FileStackToString(Dictionary<string, List<string>> fileStack) {
        string s = "";
        foreach (var dictItem in fileStack) {
            s += "Key: " + dictItem.Key + "\n";
            int i = 0;
            foreach (var listItem in dictItem.Value) {
                s += i + ": " + listItem + "\n";
            }
            s += "\n";
        }
        return s;
    }

    
    public static bool StringContains(string source, string toCheck, StringComparison comp) {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}
