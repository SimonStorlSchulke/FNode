using Godot;
using System.Collections.Generic;

public class TCFilestacks : TabContainer {

    public override void _Ready() {
        for (int i = 0; i < 6; i++) {   
            SetTabTitle(i, $"{i}");
        }
        UIUtil.ExpandTabs(this);
    }

    public FileList GetCurrentFileList() {
        return GetChild<FileList>(CurrentTab);
    }

    public void OnClearFilestack() {
        GetCurrentFileList().fileStack = new Dictionary<string, List<string>>(){
            {"Loose Files", new List<string>()},
        };
        GetCurrentFileList().UpdateUIList();
    }
}
