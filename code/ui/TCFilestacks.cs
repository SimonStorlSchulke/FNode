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
        ItemList cItemList = GetChild<ItemList>(ofStack);
        GetChild<ItemList>(ofStack).Clear();
         //Child at 0 is a secret scrollbar
        foreach (var item in Stacks[ofStack]) {
            cItemList.AddItem(item.Item2.GetFile());
            cItemList.SetItemTooltip(cItemList.GetItemCount() - 1, item.Item2);
        }
        if (Stacks[ofStack].Count > 0) {
            cItemList.GetChild<Label>(1).Visible = false;
        } else {
            cItemList.GetChild<Label>(1).Visible = true;
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
