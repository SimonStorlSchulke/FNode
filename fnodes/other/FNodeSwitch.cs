using System.IO;
using Godot;
using System;

public class FNodeSwitch : FNode
{
    OptionButton ob;
    public FNodeSwitch() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";

        // Monstrosity....
        FInput inpFalse;
        FInput inpTrue;
        FOutput outp;
        FNode.IdxReset();
        FInputBool opSwitch = new FInputBool(this);


        inpFalse = new FInputFile(this);
        inpTrue = new FInputFile(this);
    
        outp = new FOutputFile(this, delegate() {
            FileInfo valFalse = (FileInfo)inputs["False"].Get();
            FileInfo valTrue = (FileInfo)inputs["True"].Get();
            return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
        });


        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Switch", opSwitch},
            {"False", inpFalse},
            {"True", inpTrue},
        };
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"Result", outp},
        };
    }

    public override void _Ready() {
        base._Ready();

        ShowClose = true;
        Title = this.GetType().Name.Replace("FNode", "");
        this.RectMinSize = new Vector2(250, 0);
        Connect(
            "close_request", 
            GetParent(), 
            nameof(NodeTree.DeleteNode), 
            new Godot.Collections.Array(){this});

        ob = new OptionButton();
        ob.AddItem("File");
        ob.AddItem("String");
        ob.AddItem("Float");
        ob.AddItem("Int");
        ob.AddItem("Bool");
        ob.AddItem("Date");
        AddChild(ob);

        ob.Connect("item_selected", this, nameof(OptionSelected));

    }

    public void OptionSelected(int option) {

        ChangeSlotType(inputs["False"], FNode.FNodeSlotTypes.BOOL);
    }
}
