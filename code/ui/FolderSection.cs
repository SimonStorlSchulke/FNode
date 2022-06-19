using Godot;
using System;

public class FolderSection : VBoxContainer
{
    public ItemList list;
    public CheckButton cb;
    public string connecedFolder = "Loose Files";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        list = GetNode<ItemList>("List");
        cb = GetNode<CheckButton>("HBFolder/CBDropdownButton");
        cb.Connect("toggled", this, nameof(OnToggleList));
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
        foreach (string file in paths) {
            list.AddItem(file.GetFile());
        }
    }

    public void OnToggleList(bool show) {
        list.Visible = show;
    }
}
