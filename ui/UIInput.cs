using Godot;
using System;

public class UIInput : Node
{

    public override void _UnhandledInput(InputEvent e) {
        if(e.IsActionPressed("ui_delete")) {
            TCFilestacks fileStacks = Main.inst.currentProject.FileStacks;
            int selectedStack = Main.inst.currentProject.FileStacks.CurrentTab;
            ItemList il =  fileStacks.GetChild<ItemList>(selectedStack);
            int i = 0;
            foreach (int item in il.GetSelectedItems()) {
                fileStacks.Stacks[selectedStack].RemoveAt(item);
                fileStacks.OnUpdateUI(selectedStack);
                i++;
            }
        }
    }  
}
