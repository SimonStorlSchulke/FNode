using Godot;
using Godot.Collections;
using System;
using System.Linq;

public class IO : Node
{
    public static string serializeProject() {
        return "";
    }

    
    public static Dictionary<string, object> serializeNodeTree(NodeTree nt) {

        Dictionary<string, object> saveData = new Dictionary<string, object>();
        Godot.Collections.Array<Dictionary<string, object>> nodesDict = new Godot.Collections.Array<Dictionary<string, object>>();

        foreach (FNode fnd in nt.GetFNodes()) {
            nodesDict.Add(fnd.Serialize());
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

    public void Load() {
    var saveGame = new File();
    if (!saveGame.FileExists("D:/home/Desktop/thumbs/savegame.save"))
        return; // Error! We don't have a save to load.

    // Load the file line by line and process that dictionary to restore the object
    // it represents.
    saveGame.Open("D:/home/Desktop/thumbs/savegame.save", File.ModeFlags.Read);

    while (saveGame.GetPosition() < saveGame.GetLen()) {
        // Get the saved dictionary from the next line in the save file
        var saveData = new Dictionary<string, object>((Dictionary)JSON.Parse(saveGame.GetLine()).Result);

        // Now we set the remaining variables.
        Godot.Collections.Array nodesData = saveData["Nodes"] as Godot.Collections.Array;
        foreach (Dictionary nodeData in nodesData) {
            string nodeType = (string)nodeData["Type"];
            string nodeName = (string)nodeData["NodeName"];
            Vector2 offset = new Vector2((float)nodeData["OffsetX"], (float)nodeData["OffsetY"]);
            FNode fn = Main.inst.currentProject.NodeTree.OnAddNode(nodeType, offset);
            fn.Name = nodeName;
            int i = 0;
            foreach (System.Collections.DictionaryEntry inputData in (Godot.Collections.Dictionary)nodeData["Inputs"]) {
                fn.inputs[(string)inputData.Key].UpdateUIFromValue(inputData.Value);
                i++;
            }
        }

        Godot.Collections.Array connectionsData = saveData["Connections"] as Godot.Collections.Array;
        
        foreach (Godot.Collections.Dictionary cData in connectionsData) {
            GD.PrintT(cData["from"].GetType());
            GD.PrintT((System.Single)cData["from_port"]);
            GD.PrintT(cData["to"]);
            GD.PrintT(cData["to_port"]);
           
           Main.inst.currentProject.NodeTree.ConnectNode(
                (string)cData["from"],
                (int)((System.Single)cData["from_port"]),
                (string)cData["to"],
                (int)((System.Single)cData["to_port"]));
        }
    }

    saveGame.Close();
}
}
