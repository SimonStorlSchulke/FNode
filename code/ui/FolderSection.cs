using Godot;
using System.Linq;
using System.Collections.Generic;

public class FolderSection : VBoxContainer
{
    public ItemList list;
    public CheckButton cb;
    public CheckButton cbRecursive;
    public Button btnReload;
    public string connecedFolder = "Loose Files";
    [Export] NodePath NPFileList;
    FileList fileList;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        list = GetNode<ItemList>("List");
        cb = GetNode<CheckButton>("HBFolder/CBDropdownButton");
        fileList = GetParent().GetParent().GetParent<FileList>(); //TODO  dangerous. Will break when reordering FolderSection Scene
        cbRecursive = GetNode<CheckButton>("HBFolder/CBRecursive");
        btnReload = GetNode<Button>("HBFolder/BTNReload");
        cb.Connect("toggled", this, nameof(OnToggleList));
        cbRecursive.Connect("pressed", this, nameof(OnReloadFiles));
        btnReload.Connect("pressed", this, nameof(OnReloadFiles));
    }

    public void AddFile(string path) {
        list.AddItem(path.GetFile());
    }

    public void AddFiles(string[] paths) {
        foreach (string file in paths) {
            list.AddItem(file.GetFile());
        }
    }

    public void AddFiles(System.Collections.Generic.List<string> paths) {
        int i = 0;
        foreach (string file in paths) {
            list.AddItem(file.GetFile());
            list.SetItemTooltip(i, file);
            i++;
        }
    }

    public void OnReloadFiles() {
        bool recursive = cbRecursive.Pressed;
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
        list.Visible = show;
    }

        
    public override void _UnhandledInput(InputEvent e) {
        if (!cb.HasFocus()) {
            return;
        }
        if (e.IsActionPressed("ui_delete")) {
            fileList.fileStack.Remove(connecedFolder);
            QueueFree();
        }
    }
}
