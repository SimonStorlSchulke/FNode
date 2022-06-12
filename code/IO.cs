using Godot;
using Godot.Collections;
using System;

public class IO : Node
{
    public static string serializeProject() {
        return "";
    }

    
    public static Dictionary<string, object> serializeNodeTree(NodeTree nt) {

        Dictionary<string, object> saveData = new Dictionary<string, object>();
        Dictionary<string, Dictionary<string, object>> nodesDict = new Dictionary<string, Dictionary<string, object>>();

        foreach (FNode fnd in nt.GetFNodes()) {
            nodesDict.Add(fnd.Name, fnd.Serialize());
        }

        saveData.Add("Nodes", nodesDict);
        saveData.Add("Connections", nt.GetConnectionList());
        return saveData;
    }

    public void Save() {
        var saveGame = new File();
        saveGame.Open("D:/home/Desktop/thumbs/savegame.save", File.ModeFlags.Write);

        var saveData = IO.serializeNodeTree(Main.inst.currentProject.NodeTree);
        saveGame.StoreLine(JSON.Print(saveData));

        saveGame.Close();
    }
}
