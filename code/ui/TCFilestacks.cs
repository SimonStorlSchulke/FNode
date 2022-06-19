using System;
using Godot;
using System.Collections.Generic;

public class TCFilestacks : TabContainer {
    public override void _Ready() {
        for (int i = 0; i < 6; i++) {   
            SetTabTitle(i, $"  {i}  ");
        }
    }

    public FileList GetCurrentFileList() {
        return GetChild<FileList>(CurrentTab);
    }
}
