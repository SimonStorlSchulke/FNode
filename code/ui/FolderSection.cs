using Godot;
using System.Linq;

///<summary>Lists files contained in a folder - used by FileList></summary>
public class FolderSection : VBoxContainer
{
    public ItemList FileList;
    public CheckButton Cb;
    public CheckButton CbRecursive;
    public Button BtnReload;
    public string ConnecedFolder = "Loose Files";
    [Export] NodePath NPFileList;
    FileList fileList;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        FileList = GetNode<ItemList>("List");
        Cb = GetNode<CheckButton>("HBFolder/CBDropdownButton");
        fileList = GetParent().GetParent().GetParent<FileList>(); //TODO  dangerous. Will break when reordering FolderSection Scene
        CbRecursive = GetNode<CheckButton>("HBFolder/CBRecursive");
        BtnReload = GetNode<Button>("HBFolder/BTNReload");
        Cb.Connect("toggled", this, nameof(OnToggleList));
        CbRecursive.Connect("pressed", this, nameof(OnReloadFiles));
        BtnReload.Connect("pressed", this, nameof(OnReloadFiles));
    }

    public void AddFile(string path) {
        FileList.AddItem(path.GetFile());
    }

    public void AddFiles(string[] paths) {
        foreach (string file in paths) {
            FileList.AddItem(file.GetFile());
        }
    }

    public void AddFiles(System.Collections.Generic.List<string> paths) {
        int i = 0;
        foreach (string file in paths) {
            FileList.AddItem(file.GetFile());
            FileList.SetItemTooltip(i, file);
            i++;
        }
    }

    public void OnReloadFiles() {
        bool recursive = CbRecursive.Pressed;
        int idx = GetIndex();
        string path = fileList.fileStack.ElementAt(idx).Key;
        fileList.fileStack.ElementAt(idx).Value.Clear();

        string[] dirFiles;
        if (recursive) {
            dirFiles = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);
        } else {
            dirFiles = System.IO.Directory.GetFiles(path);
        }
        fileList.fileStack.ElementAt(idx).Value.AddRange(dirFiles);
        fileList.UpdateUIListAtKey(path);
    }

    public void OnToggleList(bool show) {
        FileList.Visible = show;
    }

        
    public override void _UnhandledInput(InputEvent e) {
        if (!Cb.HasFocus()) {
            return;
        }
        if (e.IsActionPressed("ui_delete")) {
            fileList.fileStack.Remove(ConnecedFolder);
            QueueFree();
        }
    }
}
