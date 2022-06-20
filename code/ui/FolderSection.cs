using Godot;
using System.Linq;
using System.Collections.Generic;

public class FolderSection : VBoxContainer
{
    public ItemList list;
    public CheckButton cb;
    public CheckButton cbRecursive;
    public TextureButton btnReload;
    public string connecedFolder = "Loose Files";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        list = GetNode<ItemList>("List");
        cb = GetNode<CheckButton>("HBFolder/CBDropdownButton");
        cbRecursive = GetNode<CheckButton>("HBFolder/CBRecursive");
        btnReload = GetNode<TextureButton>("HBFolder/BTNReload");
        cb.Connect("toggled", this, nameof(OnToggleList));
        cbRecursive.Connect("pressed", this, nameof(OnToggleRecursive));
        btnReload.Connect("pressed", this, nameof(OnToggleRecursive));
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

    public void OnToggleRecursive() {
        bool recursive = cbRecursive.Pressed;
        int idx = GetIndex();
        FileList fl = GetParent().GetParent().GetParent<FileList>(); //TODO  dangerous. Will break when reordering FolderSection Scene
        string path = fl.fileStack.ElementAt(idx).Key;
        fl.fileStack.ElementAt(idx).Value.Clear();

        string[] dirFiles;
        if (recursive) {
            dirFiles = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);
        } else {
            dirFiles = System.IO.Directory.GetFiles(path);
        }
        fl.fileStack.ElementAt(idx).Value.AddRange(dirFiles);
        fl.UpdateUIListAtKey(path);
    }

    public void OnToggleList(bool show) {
        list.Visible = show;
    }
}
