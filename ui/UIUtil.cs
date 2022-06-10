using System.Drawing;
using Godot;
using System;
using System.Collections.Generic;

public class UIUtil : Node
{

    static Dictionary<string, StyleBox> styleboxes = new Dictionary<string, StyleBox>();
    
    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree() {
        styleboxes.Add("NodeFile", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeFile.stylebox"));
        styleboxes.Add("NodeFile_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeFile_Selected.stylebox"));
        styleboxes.Add("NodeString", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeString.stylebox"));
        styleboxes.Add("NodeString_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeString_Selected.stylebox"));
        styleboxes.Add("NodeDate", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeDate.stylebox"));
        styleboxes.Add("NodeDate_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeDate_Selected.stylebox"));
        styleboxes.Add("NodeMath", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeMath.stylebox"));
        styleboxes.Add("NodeMath_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeMath_Selected.stylebox"));
        styleboxes.Add("NodeOther", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeBool.stylebox"));
        styleboxes.Add("NodeOther_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeOther_Selected.stylebox"));
        styleboxes.Add("NodeBool", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeOther.stylebox"));
        styleboxes.Add("NodeBool_Selected", ResourceLoader.Load<StyleBox>("res://ui/NodeStyles/NodeBool_Selected.stylebox"));
    }

    public static void CreateUI(FNode fn) {

    switch (fn.category)
    {
        case "File":
            fn.AddStyleboxOverride("frame", styleboxes["NodeFile"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeFile_Selected"]);
            break;

        case "String":
            fn.AddStyleboxOverride("frame", styleboxes["NodeString"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeString_Selected"]);
            break;

        case "Date":
        GD.Print("Date!");
        GD.Print((styleboxes["NodeDate"] as StyleBoxFlat).BorderColor);
            fn.AddStyleboxOverride("frame", styleboxes["NodeDate"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeDate_Selected"]);
            break;

        case "Math":
            fn.AddStyleboxOverride("frame", styleboxes["NodeMath"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeMath_Selected"]);
            break;


        case "Bool":
            fn.AddStyleboxOverride("frame", styleboxes["NodeBool"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeBool_Selected"]);
            break;

        default:
            fn.AddStyleboxOverride("frame", styleboxes["NodeOther"]);
            fn.AddStyleboxOverride("selectedframe", styleboxes["NodeOther_Selected"]);
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
        var hb = new HBoxContainer();
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

            case FOutputDate t5:
                slotColor = Colors.LightGreen;
                break;

            default:
                slotColor = Colors.Black;
                break;
        }
        Control ct = new Control();
        ct.SizeFlagsHorizontal = 2;
        hb.AddChild(ct);
        hb.AddChild(lb);
        hb.RectMinSize = new Vector2(0, 40);
        toNode.AddChild(hb);
        

        if (atIdx != -1) {
            toNode.SetSlot(atIdx, false, 0, slotColor, true, 0, slotColor, null, null);
            toNode.MoveChild(hb, atIdx);
        } else {
            toNode.SetSlot(hb.GetIndex(), false, 0, slotColor, true, 0, slotColor, null, null);
        }
    }

    public static void AddInputUI(FNode toNode, string labeltext, FInput fInp, int atIdx = -1) {
        var hb = new HBoxContainer();
        var lb = new Label();
        lb.Text = labeltext;
        lb.RectMinSize = new Vector2(130, 0);
        hb.AddChild(lb);
        Control ct;
        Godot.Color slotColor;
        switch (fInp)
        {
            case FInputInt t1:
                ct = new SpinBox();
                slotColor = Colors.SeaGreen;
                break;
                
            case FInputFloat t2:
                ct = new SpinBox();
                ((SpinBox)ct).Step = 0.01;
                slotColor = Colors.SkyBlue;
                break;

            case FInputBool t3:
                ct = new CheckBox();
                slotColor = Colors.LightPink;
                break;

            case FInputString t4:
                ct = new LineEdit();
                slotColor = Colors.Orange;
                break;

            case FInputFile t5:
                ct = new LineEdit();
                slotColor = Colors.Red;
                break;

            case FInputDate t5:
                GDScript calendarbutonClass = (GDScript) GD.Load("res://addons/calendar_button/scripts/calendar_script.gd");
                DateLabel lblDate = new DateLabel();
                lblDate.Text = DateTime.Now.ToString();
                lblDate.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
                hb.AddChild(lblDate);
                ct = (Control) calendarbutonClass.New();
                ct.Name = "clButton";
                ct.Connect("date_selected", lblDate, nameof(DateLabel.SetDate));
                slotColor = Colors.LightGreen;
                break;

            default:
                ct = new LineEdit();
                slotColor = Colors.Black;
                break;
        }

        if (ct.Name != "clButton")
            ct.SizeFlagsHorizontal = 3;

        hb.AddChild(ct);
        hb.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        hb.RectMinSize = new Vector2(0, 40);
        toNode.AddChild(hb);

        if (atIdx != -1) {
            toNode.SetSlot(atIdx, true, 0, slotColor, false, 0, Colors.Red, null, null);
            toNode.MoveChild(hb, atIdx);
        } else {
            toNode.SetSlot(hb.GetIndex(), true, 0, slotColor, false, 0, Colors.Red, null, null);
        }
    }
}

class DateLabel : Label {
    public void SetDate(Godot.Reference date) {
        Text = (string)date.Call("date", new object[]{"DD.MM.YYYY"});
    }
}