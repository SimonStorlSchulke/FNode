using System.Collections.Generic;
using Godot;

public partial class IO : Node {

    [Export] NodePath NPFDSave;
    FileDialog FDSave;
    [Export] NodePath NPFDLoad;
    FileDialog FDLoad;

    public override void _Ready() {
        FDSave = GetNode<FileDialog>(NPFDSave);
        FDSave.Connect("file_selected", new Callable(this, nameof(OnSaveFileSelected)));
        FDLoad = GetNode<FileDialog>(NPFDLoad);
        FDLoad.Connect("file_selected", new Callable(this, nameof(OnLoadFileSelected)));
        FDLoad.CurrentDir = FDSave.CurrentDir = GetDefaultSaveDir();
    }

    public string GetDefaultSaveDir() {
        //Using Appdata for APPLICATION DATA... not littering the users Documents Folder like every goddamn other App does!
        string path = FileUtil.JoinPaths(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "FNode/saves");
        FileUtil.CreateDirIfNotExisting(path);
        return path;
    }

    public void OnLoadFileSelected(string path) {
        Load(path);
    }

    public void OnSaveFileSelected(string path) {
        Save(path);
    }

    public void OnPopupLoad() {
        FDLoad.PopupCentered();
        FDLoad.CurrentDir = FDLoad.CurrentDir; //refresh files
    }

    public void OnPopupSave() {
        FDSave.PopupCentered();
        FDSave.CurrentDir = FDSave.CurrentDir; //refresh files
    }

    public static Dictionary<string, object> serializeProject(NodeTree nt) {

        Dictionary<string, object> saveData = new Dictionary<string, object>();
        List<Dictionary<string, object>> nodesDict = new ();

      //  foreach (FNode fnd in nt.GetFNodes()) {
      //      nodesDict.Add(fnd.Serialize());
      //  }

        saveData.Add("Nodes", nodesDict);
        saveData.Add("Connections", nt.GetConnectionList());
        saveData.Add("iterations", Main.Inst.CurrentProject.spIterations.Value);
        saveData.Add("ScrollOffsetX", Main.Inst.CurrentProject.NodeTree.ScrollOffset.X);
        saveData.Add("ScrollOffsetY", Main.Inst.CurrentProject.NodeTree.ScrollOffset.Y);
        return saveData;
    }

    public static Dictionary<string, object> CopySelectedNodes(NodeTree nt) {
        Dictionary<string, object> copyData = new Dictionary<string, object>();
        /*
        Dictionary<string, object> nodesDict = new();

        foreach (FNode fnd in nt.GetSelectedNodes()) {
            nodesDict.Add(fnd.Name, fnd.Serialize());
        }

        copyData.Add("Nodes", nodesDict);

        var connectionList = nt.GetConnectionList();

        foreach (Dictionary connection in nt.GetConnectionList()) {
            
            // If a connection is links to an unconnected node, remove it from the connectionList
            if (!nodesDict.ContainsKey(connection["from"] as string) || !nodesDict.ContainsKey(connection["to"] as string)) {
                connectionList.Remove(connection);
            }
        }
        copyData.Add("Connections", connectionList);
        //OS.Clipboard = JSON.Print(copyData); //TODO migration
        */
        return copyData;
    }

    public static void PasteNodes() {
        /*
        var pastedData = new Dictionary<string, object>((Dictionary)JSON.Parse(OS.Clipboard).Result);
        Dictionary<string,object> nodesData = pastedData["Nodes"] as  Dictionary<string,object>;

        foreach (KeyValuePair<string, object> nodeData in nodesData) {
            FNode.Deserialize(nodeData.Value as Godot.Collections.Dictionary, Main.Inst.CurrentProject);
        }

        Godot.Collections.Array connectionsData = pastedData["Connections"] as Godot.Collections.Array;

        foreach (Godot.Collections.Dictionary cData in connectionsData) {
                Main.Inst.CurrentProject.NodeTree.OnConnectionRequest(
                     (string)cData["from"],
                     (int)((System.Single)cData["from_port"]),
                     (string)cData["to"],
                     (int)((System.Single)cData["to_port"]));
            }
            */
    }

    public void Save(string path) {
        //TODO migration
      // var saveData = IO.serializeProject(Main.Inst.CurrentProject.NodeTree);

      // var saveGame = new File();
      // saveGame.Open(path, File.ModeFlags.Write);
      // saveGame.StoreLine(JSON.Print(saveData));
      // saveGame.Close();

      // InfoLine.Show($"Saved Project to {path}");
      // Main.Inst.ProjectTabs.SetTabTitle(Main.Inst.ProjectTabs.CurrentTab, System.IO.Path.GetFileNameWithoutExtension(path));
    }

    public void Load(string path) { /* TODO migration
        var saveGame = new File();
        if (!saveGame.FileExists(path))
            return; // Error! We don't have a save to load.

        // Load the file line by line and process that dictionary to restore the object
        // it represents.
        saveGame.Open(path, File.ModeFlags.Read);


        Project pr = Main.NewProject(System.IO.Path.GetFileNameWithoutExtension(path));

        while (saveGame.GetPosition() < saveGame.GetLength()) {
            var saveData = new Dictionary<string, object>((Dictionary)JSON.Parse(saveGame.GetLine()).Result);

            try {
                Godot.Collections.Array nodesData = saveData["Nodes"] as Godot.Collections.Array;
            foreach (Dictionary nodeData in nodesData) {
                FNode.Deserialize(nodeData, pr);
            }

            Godot.Collections.Array connectionsData = saveData["Connections"] as Godot.Collections.Array;

            foreach (Godot.Collections.Dictionary cData in connectionsData) {
                pr.NodeTree.OnConnectionRequest(
                     (string)cData["from"],
                     (int)((System.Single)cData["from_port"]),
                     (string)cData["to"],
                     (int)((System.Single)cData["to_port"]));
            }

            pr.NodeTree.ScrollOffset = new Vector2((float)(System.Single)saveData["ScrollOffsetX"], (float)(System.Single)saveData["ScrollOffsetY"]);

            Main.Inst.CurrentProject.spIterations.Value = (int)(System.Single)saveData["iterations"];
            InfoLine.Show($"Loaded Project {path}");

            } catch (System.Exception e) {
                Errorlog.Log("Error loading Project: ", e);
            }

        }

        saveGame.Close(); */
    }
}
