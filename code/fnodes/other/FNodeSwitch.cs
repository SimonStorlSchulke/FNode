using System.IO;
using Godot;
using System;


// TODO - unused for now because of problems with (De)serialization. How to save changed slottypes?
public partial class FNodeSwitch : FNode
{
    OptionButton ob;

    public FNodeSwitch() {
        TooltipText = "Switch between the values of the False and True inputs depending on the state of the Switch input.";
        category = "Other";


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
                FileInfo valFalse = inputs["False"].Get<FileInfo>();
                FileInfo valTrue = inputs["True"].Get<FileInfo>();
                return (FileInfo)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
            })},
        };
    }

    public override void _Ready() {
        base._Ready();

        AddOptionEnum(
            "Mode",

            new string[] {
                "File",
                "Text",
                "Bool",
                "Int",
                "Float",
                "Date",
                "List",
                "Image",
            }, nameof(OptionSelected));
    }

    public void OptionSelected(int option) {

        var oldConList = outputs["Result"].GetConnectedInputs();

        //This is a monstrosity- is there a better way?

        ChangeSlotType(inputs["True"], (FNode.FNodeSlotTypes)option);
        ChangeSlotType(inputs["False"], (FNode.FNodeSlotTypes)option);

        switch (option)
        {
            case (int)(FNodeSlotTypes.FILE):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        FileInfo valFalse = inputs["False"].Get<FileInfo>();
                        FileInfo valTrue = inputs["True"].Get<FileInfo>();
                        return (FileInfo)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.STRING):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        string valFalse = inputs["False"].Get<string>();
                        string valTrue = inputs["True"].Get<string>();
                        return (string)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.BOOL):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        bool valFalse = inputs["False"].Get<bool>();
                        bool valTrue = inputs["True"].Get<bool>();
                        return (bool)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.INT):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        int valFalse = inputs["False"].Get<int>();
                        int valTrue = inputs["True"].Get<int>();
                        return (int)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.FLOAT):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        float valFalse = inputs["False"].Get<float>();
                        float valTrue = inputs["True"].Get<float>();
                        return (float)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.DATE):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        DateTime valFalse = inputs["False"].Get<DateTime>();
                        DateTime valTrue = inputs["True"].Get<DateTime>();
                        return (DateTime)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.LIST):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        Godot.Collections.Array valFalse = inputs["False"].Get<Godot.Collections.Array>();
                        Godot.Collections.Array valTrue = inputs["True"].Get<Godot.Collections.Array>();
                        return (Godot.Collections.Array)(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
            case (int)(FNodeSlotTypes.IMAGE):
                ChangeSlotType(
                    outputs["Result"], 
                    delegate() {
                        ImageMagick.MagickImage valFalse = inputs["False"].Get<ImageMagick.MagickImage >();
                        ImageMagick.MagickImage valTrue = inputs["True"].Get<ImageMagick.MagickImage >();
                        return (ImageMagick.MagickImage )(inputs["Switch"].Get<bool>() ? valTrue : valFalse);
                    }, 
                    (FNode.FNodeSlotTypes)option);
                break;
        }
        foreach (var cTo in oldConList){
            cTo.connectedTo = outputs["Result"];
        }
    }
}
