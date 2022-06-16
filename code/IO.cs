using Godot;
using Godot.Collections;

public class IO : Node {

    [Export] NodePath NPFDSave;
    FileDialog FDSave;
    [Export] NodePath NPFDLoad;
    FileDialog FDLoad;

    public override void _Ready() {
        FDSave = GetNode<FileDialog>(NPFDSave);
        FDSave.Connect("file_selected", this, nameof(OnSaveFileSelected));
        FDLoad = GetNode<FileDialog>(NPFDLoad);
        FDLoad.Connect("file_selected", this, nameof(OnLoadFileSelected));
        FDLoad.CurrentDir = FDSave.CurrentDir = GetDefaultSaveDir();
    }

    public string GetDefaultSaveDir() {
        string path = FileUtil.JoinPaths(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "FNode");
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

    public void Save(string path) {
        var saveGame = new File();
        saveGame.Open(path, File.ModeFlags.Write);

        var saveData = IO.serializeNodeTree(Main.inst.currentProject.NodeTree);

        saveData.Add("iterations", Main.inst.currentProject.spIterations.Value);
        saveGame.StoreLine(JSON.Print(saveData));

        saveGame.Close();
        InfoLine.Show($"Saved Project to {path}");
        Main.inst.projectTabs.GetChild(Main.inst.projectTabs.CurrentTab).Name = System.IO.Path.GetFileNameWithoutExtension(path);
    }

    public void Load(string path) {
        var saveGame = new File();
        if (!saveGame.FileExists(path))
            return; // Error! We don't have a save to load.

        // Load the file line by line and process that dictionary to restore the object
        // it represents.
        saveGame.Open(path, File.ModeFlags.Read);


        Project pr = Main.NewProject(System.IO.Path.GetFileNameWithoutExtension(path));

        while (saveGame.GetPosition() < saveGame.GetLen()) {
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

            Main.inst.currentProject.spIterations.Value = (int)(System.Single)saveData["iterations"];
            InfoLine.Show($"Loaded Project {path}");

            } catch (System.Exception e) {
                Errorlog.Log("Error loading Project: ", e);
            }

        }

        saveGame.Close();
    }
}
