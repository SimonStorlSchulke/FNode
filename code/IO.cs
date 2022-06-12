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
    }

    public void OnLoadFileSelected(string path) {
        GD.Print("Loading");
        Load(path);
    }

    public void OnSaveFileSelected(string path) {
        GD.Print("Saving");
        Save(path);
    }

    public void OnPopupLoad() {
        FDLoad.PopupCentered();
    }

    public void OnPopupSave() {
        FDSave.PopupCentered();
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
        saveGame.StoreLine(JSON.Print(saveData));

        saveGame.Close();
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
            // Get the saved dictionary from the next line in the save file
            var saveData = new Dictionary<string, object>((Dictionary)JSON.Parse(saveGame.GetLine()).Result);

            // Now we set the remaining variables.
            Godot.Collections.Array nodesData = saveData["Nodes"] as Godot.Collections.Array;
            foreach (Dictionary nodeData in nodesData) {
                string nodeType = (string)nodeData["Type"];
                string nodeName = (string)nodeData["NodeName"];
                Vector2 offset = new Vector2((float)nodeData["OffsetX"], (float)nodeData["OffsetY"]);
                FNode fn = pr.NodeTree.OnAddNode(nodeType, offset);
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
                
                pr.NodeTree.OnConnectionRequest(
                     (string)cData["from"],
                     (int)((System.Single)cData["from_port"]),
                     (string)cData["to"],
                     (int)((System.Single)cData["to_port"]));
            }
        }

        saveGame.Close();
    }
}
