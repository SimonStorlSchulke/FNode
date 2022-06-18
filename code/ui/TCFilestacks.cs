using System;
using Godot;
using System.Collections.Generic;

public class TCFilestacks : TabContainer
{
    //List of Filelists. bool is True if it's a Folder.
    public List<List<Tuple<bool, string>>> Stacks = new List<List<Tuple<bool, string>>>() {
        new List<Tuple<bool, string>>(),
        new List<Tuple<bool, string>>(),
        new List<Tuple<bool, string>>(),
        new List<Tuple<bool, string>>(),
        new List<Tuple<bool, string>>(),
        new List<Tuple<bool, string>>(),
    }; //TODO Refactor to variable length of Stacks

    public override void _Ready() {
        for (int i = 0; i < 6; i++) {   
            SetTabTitle(i, $"  {i}  ");
        }
    }

    public void OnUpdateUI(int ofStack) {
        GetChild<ItemList>(ofStack).Clear();
        GetChild<ItemList>(ofStack).GetChild<Label>(1).Visible = false; //Child at 0 is a secret scrollbar
        foreach (var item in Stacks[ofStack]) {
            GetChild<ItemList>(ofStack).AddItem(item.Item2.GetFile());
        }
    }

    public void AddFile(string file, int toStack) {
        Stacks[toStack].Add(new Tuple<bool, string>(false, file));
    }

    public void OnClearFilestack() {
        Stacks[CurrentTab].Clear();
        OnUpdateUI(CurrentTab);
    }
}
