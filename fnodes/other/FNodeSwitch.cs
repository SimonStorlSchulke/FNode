using System.IO;
using Godot;
using System;

public class FNodeSwitch : FNode
{
    OptionButton ob;
    public FNodeSwitch() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";
    }

    public override void _Ready() {
        base._Ready();
        ob = new OptionButton();
        ob.AddItem("File");
        ob.AddItem("String");
        ob.AddItem("Float");
        ob.AddItem("Int");
        ob.AddItem("Bool");
        ob.AddItem("Date");

        ob.Connect("item_selected", this, nameof(OptionSelected));
        OptionSelected(0);

    }

    public void OptionSelected(int option) {
        GD.Print("CALLING!");
        // Monstrosity....
        FInput inpFalse;
        FInput inpTrue;
        FOutput outp;

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Switch", new FInputBool(this)},
        };

        switch (option)
        {
            case 0:
                inpFalse = new FInputFile(this);
                inpTrue = new FInputFile(this);
            
                outp = new FOutputFile(this, delegate() {
                    GD.Print("ME NO STRING!");
                    FileInfo valFalse = (FileInfo)inputs["False"].Get();
                    FileInfo valTrue = (FileInfo)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
            case 1:
                inpFalse = new FInputString(this);
                inpTrue = new FInputString(this);
                GD.Print("ME STRING!");
                outp = new FOutputString(this, delegate() {
                    string valFalse = (string)inputs["False"].Get();
                    string valTrue = (string)inputs["True"].Get();
                    return (string)((bool)inputs["Switch"].Get() ? valTrue : valFalse);
                });
                break;
            case 2:
                inpFalse = new FInputFloat(this);
                inpTrue = new FInputFloat(this);
            
                outp = new FOutputFloat(this, delegate() {
                    float valFalse = (float)inputs["False"].Get();
                    float valTrue = (float)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
            case 3:
                inpFalse = new FInputInt(this);
                inpTrue = new FInputInt(this);
            
                outp = new FOutputInt(this, delegate() {
                    int valFalse = (int)inputs["False"].Get();
                    int valTrue = (int)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
            case 4:
                inpFalse = new FInputBool(this);
                inpTrue = new FInputBool(this);
            
                outp = new FOutputBool(this, delegate() {
                    bool valFalse = (bool)inputs["False"].Get();
                    bool valTrue = (bool)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
            case 5:
                inpFalse = new FInputDate(this);
                inpTrue = new FInputDate(this);
            
                outp = new FOutputDate(this, delegate() {
                    DateTime valFalse = (DateTime)inputs["False"].Get();
                    DateTime valTrue = (DateTime)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
            default:
                inpFalse = new FInputFile(this);
                inpTrue = new FInputFile(this);
            
                outp = new FOutputFloat(this, delegate() {
                    FileInfo valFalse = (FileInfo)inputs["False"].Get();
                    FileInfo valTrue = (FileInfo)inputs["True"].Get();
                    return (bool)inputs["Switch"].Get() ? valTrue : valFalse;
                });
                break;
        }

        inputs.Add("False", inpFalse);
        inputs.Add("True", inpTrue);

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {"Result", outp},
        };

        if (GetChildCount() > 1) {
            GetChild(0).QueueFree();
            GetChild(1).QueueFree();
            GetChild(2).QueueFree();
            GetChild(3).QueueFree();
            RemoveChild(ob);
        }

        UIUtil.CreateUI(this);
        GD.Print(inputs.Count);
        AddChild(ob);
       //UIUtil.AddInputUI(this, "False", inpFalse);
        //UIUtil.AddInputUI(this, "True", inpTrue);
    }
}
