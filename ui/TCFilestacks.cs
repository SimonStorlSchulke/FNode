using System;
using Godot;
using System.Collections.Generic;

public class TCFilestacks : TabContainer
{
    //List of Filelists. bool is True if it's a Folder.
    public List<List<Tuple<bool, string>>> Stacks = new List<List<Tuple<bool, string>>>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
    }

    public void OnUpdateUI(int ofStack) {
        GetChild<ItemList>(ofStack).Clear();
        foreach (var item in Stacks[ofStack]) {
             GetChild<ItemList>(ofStack).AddItem(item.Item2.GetFile());
        }
    }
}
