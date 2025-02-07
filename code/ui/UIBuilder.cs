using Godot;
using System;
using System.Collections.Generic;


public partial class UIBuilder : Node
{
        static Dictionary<string, StyleBox> styleboxes = new Dictionary<string, StyleBox>(){
        {"NodeFile", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeFile.stylebox")},
        {"NodeFile_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeFile_Selected.stylebox")},
        {"NodeString", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeString.stylebox")},
        {"NodeString_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeString_Selected.stylebox")},
        {"NodeDate", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeDate.stylebox")},
        {"NodeDate_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeDate_Selected.stylebox")},
        {"NodeMath", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeMath.stylebox")},
        {"NodeMath_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeMath_Selected.stylebox")},
        {"NodeBool", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeBool.stylebox")},
        {"NodeBool_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeBool_Selected.stylebox")},
        {"NodeList", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeList.stylebox")},
        {"NodeList_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeList_Selected.stylebox")},
        {"NodeImage", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeImage.stylebox")},
        {"NodeImage_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeImage_Selected.stylebox")},
        {"NodeOther", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeOther.stylebox")},
        {"NodeOther_Selected", ResourceLoader.Load<StyleBox>("res://theme/NodeStyles/NodeOther_Selected.stylebox")},
    };

    public static void CreateUI(FNode fn) {

    switch (fn.category)
    {
        case "File":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeFile"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeFile_Selected"]);
            break;

        case "Text":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeString"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeString_Selected"]);
            break;

        case "Date":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeDate"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeDate_Selected"]);
            break;

        case "Math":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeMath"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeMath_Selected"]);
            break;


        case "Bool":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeBool"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeBool_Selected"]);
            break;

        case "List":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeList"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeList_Selected"]);
            break;

        case "Img":
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeImage"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeImage_Selected"]);
            break;

        default:
            fn.AddThemeStyleboxOverride("frame", styleboxes["NodeOther"]);
            fn.AddThemeStyleboxOverride("selectedframe", styleboxes["NodeOther_Selected"]);
            break;
    }

        foreach (var item in fn.outputs) {
            AddOutputUI(fn, item.Key, item.Value);
        }

        foreach (var item in fn.inputs) {
            AddInputUI(fn, item.Key, item.Value);
        }
    }

    public static void AddOutputUI(FNode toNode, string labeltext, FOutput fOutp, int atIdx = -1) {
        var hb = new HbOutput();
        hb.TooltipText = fOutp.description;
        var lb = new Label();
        lb.Text = labeltext;
        Godot.Color slotColor;
        switch (fOutp)
        {
            case FOutputInt t1:
                slotColor = Colors.SeaGreen;
                break;
                
            case FOutputFloat t2:
                slotColor = Colors.SkyBlue;
                break;

            case FOutputBool t3:
                slotColor = Colors.LightPink;
                break;

            case FOutputString t4:
                slotColor = Colors.Orange;
                break;

            case FOutputFile t5:
                slotColor = Colors.Red;
                break;

            case FOutputList t5:
                slotColor = Colors.Purple;
                break;

            case FOutputDate t5:
                slotColor = Colors.LightGreen;
                break;

            case FOutputImage t5:
                slotColor = Colors.Blue;
                break;

            case FOutputColor t6:
                slotColor = Colors.BlueViolet;
                break;

            default:
                slotColor = Colors.White;
                break;
        }
        Control ct = new Control();
        ct.SizeFlagsHorizontal = Control.SizeFlags.Expand;
        hb.AddChild(ct);
        hb.AddChild(lb);
        hb.CustomMinimumSize = new Vector2(0, 40);
        toNode.AddChild(hb);
        

        if (atIdx != -1) {
            toNode.SetSlot(atIdx, false, 0, slotColor, true, 0, slotColor, null, null);
            toNode.MoveChild(hb, atIdx);
        } else {
            toNode.SetSlot(hb.GetIndex(), false, 0, slotColor, true, 0, slotColor, null, null);
        }
    }

    public static void AddInputUI(FNode toNode, string labeltext, FInput fInp, int atIdx = -1) {
        var hb = new HbInput();
        var lb = new Label();
        lb.Text = labeltext;
        lb.CustomMinimumSize = new Vector2(130, 0);
        hb.AddChild(lb);
        Control ct;
        Godot.Color slotColor;
        switch (fInp)
        {
            case FInputInt inpInt:
                ct = new SpinBox();
                (ct as SpinBox).MinValue = inpInt.min;
                (ct as SpinBox).MaxValue = inpInt.max;
                if(fInp.initialValue != null) (ct as SpinBox).Value = (int)fInp.initialValue;
                slotColor = Colors.SeaGreen;
                break;
                
            case FInputFloat inpFloat:
                ct = new SpinBox();
                (ct as SpinBox).MinValue = inpFloat.min;
                (ct as SpinBox).MaxValue = inpFloat.max;
                ((SpinBox)ct).Step = 0.01;
                if(fInp.initialValue != null) (ct as SpinBox).Value = (float)fInp.initialValue;
                slotColor = Colors.SkyBlue;
                break;

            case FInputBool inpBool:
                ct = new CheckBox();
                if(fInp.initialValue != null) (ct as CheckBox).SetPressed((bool)fInp.initialValue);
                slotColor = Colors.LightPink;
                break;

            case FInputString inpString:
                ct = new LineEdit();
                Button btnTextEditor = new Button();
                btnTextEditor.Text = " ^ ";
                btnTextEditor.Pressed += () => TextEditor.inst.ShowEditor(fInp.owner, labeltext);
                hb.AddChild(btnTextEditor);
                if(fInp.initialValue != null) (ct as LineEdit).Text = (string)fInp.initialValue;
                slotColor = Colors.Orange;
                break;

            case FInputFile inpFile:
                ct = new LineEdit();
                if(fInp.initialValue != null) (ct as LineEdit).Text = (string)fInp.initialValue;
                slotColor = Colors.Red;
                break;

            case FInputImage inpImage:
                ct = new Control();
                slotColor = Colors.Blue;
                break;

            case FInputList t5:
                //ct = new Label();
                //(ct as Label).Text = "connect List Slot"; //(string)fInp.initialValue;
                ct = new Button();
                ((Button)ct).Text = "Edit List"; 
                //This might break...:
                ((Button)ct).Pressed += () => ListCreator.inst.ShowCreator(fInp.DefaultValue as Godot.Collections.Array, fInp.owner, labeltext);
                slotColor = Colors.Purple;
                break;

            case FInputDate t5:
                GDScript calendarbutonClass = (GDScript) GD.Load("res://addons/calendar_button/scripts/calendar_script.gd");
                DateLabel lblDate = new DateLabel();
                lblDate.Text = DateTime.Now.ToString();
                lblDate.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                hb.AddChild(lblDate);
                ct = (Control) calendarbutonClass.New();
                ct.Name = "clButton";
                ct.Connect("date_selected", new Callable(lblDate, nameof(DateLabel.SetDate)));
                slotColor = Colors.LightGreen;
                break;

            case FInputColor t6:
                ct = new ColorPickerButton();
                if(fInp.initialValue != null) (ct as ColorPickerButton).Color = (Color)fInp.initialValue;
                slotColor = Colors.BlueViolet;
                break;

            default:
                ct = new Control();
                slotColor = Colors.White;
                break;
        }

        if (ct.Name != "clButton")
            ct.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        hb.TooltipText = ct.TooltipText = fInp.description;

        hb.AddChild(ct);
        hb.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        hb.CustomMinimumSize = new Vector2(0, 40);
        toNode.AddChild(hb);
        hb.Name = labeltext;

        if (atIdx != -1) {
            toNode.SetSlot(atIdx, true, 0, slotColor, false, 0, Colors.Red, null, null);
            toNode.MoveChild(hb, atIdx);
        } else {
            toNode.SetSlot(hb.GetIndex(), true, 0, slotColor, false, 0, Colors.Red, null, null);
        }
    }
}

