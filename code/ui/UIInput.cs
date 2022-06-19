using Godot;
using System.Linq;
using System.Collections.Generic;

public class UIInput : Node
{

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_delete")) {
            TCFilestacks fileStacks = Main.inst.currentProject.FileStacks;
            int selectedStack = Main.inst.currentProject.FileStacks.CurrentTab;
            fileStacks.GetCurrentFileList().RemoveSelectedItems();
            //fileStacks.Stacks[selectedStack].RemoveAll(i => selItems.Contains(fileStacks.Stacks[selectedStack].IndexOf(i)));
            //fileStacks.OnUpdateUI(selectedStack);
        }
    }
}
