using System.IO;
using Godot;
using System;



public class FNodeSwitch : FNode
{
    OptionButton ob;

    public FNodeSwitch() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";


        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Switch", new FInputBool(this)},
            {"False", new FInputFile(this)},
            {"True", new FInputFile(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"Result", 
            new FOutputFile(this, delegate() {
                FileInfo valFalse = (FileInfo)inputs["False"].Get();
                FileInfo valTrue = (FileInfo)inputs["True"].Get();
                return (FileInfo)((bool)inputs["Switch"].Get() ? valTrue : valFalse);
            })},
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
        ob.AddItem("Bool");
        ob.AddItem("Int");
        ob.AddItem("Float");
        ob.AddItem("Date");
        AddChild(ob);

        ob.Connect("item_selected", this, nameof(OptionSelected));

    }

    public void OptionSelected(int option) {

        var oldConList = outputs["Result"].ConnectedTo();

        //This is a monstrosity- is there a better way?

        ChangeSlotType(inputs["True"], (FNode.FNodeSlotTypes)option);
        ChangeSlotType(inputs["False"], (FNode.FNodeSlotTypes)option);

        switch (option)
        {
            case (int)(FNodeSlotTypes.FILE):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        FileInfo valFalse = (FileInfo)inputs["False"].Get();
                        FileInfo valTrue = (FileInfo)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.STRING):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        string valFalse = (string)inputs["False"].Get();
                        string valTrue = (string)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.BOOL):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        bool valFalse = (bool)inputs["False"].Get();
                        bool valTrue = (bool)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.INT):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        int valFalse = (int)inputs["False"].Get();
                        int valTrue = (int)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.FLOAT):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        float valFalse = (float)inputs["False"].Get();
                        float valTrue = (float)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.DATE):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        DateTime valFalse = (DateTime)inputs["False"].Get();
                        DateTime valTrue = (DateTime)inputs["True"].Get();
                        return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
        }
        foreach (var cTo in oldConList){
            cTo.connectedTo = outputs["Result"];
        }
    }
}
