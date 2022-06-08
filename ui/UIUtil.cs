using System.Drawing;
using Godot;
using System;

public class UIUtil : Node
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public static Control CreateUI(FNode fn) {

        foreach (var item in fn.outputs)
        {
            AddOutputUI(fn, item.Key, item.Value);
        }

        foreach (var item in fn.inputs)
        {
            AddInputUI(fn, item.Key, item.Value);
        }
        return new Control();
    }

    public static void AddOutputUI(FNode toNode, string labeltext, FOutput fOutp) {
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
                slotColor = Colors.DarkBlue;
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
        toNode.AddChild(hb);
        toNode.SetSlot(hb.GetIndex(), false, 0, slotColor, true, 0, slotColor, null, null);
    }

    public static void AddInputUI(FNode toNode, string labeltext, FInput fInp) {
        var hb = new HBoxContainer();
        var lb = new Label();
        lb.Text = labeltext;
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
                slotColor = Colors.DarkBlue;
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
                Label lblDate = new Label();
                lblDate.Text = DateTime.Now.ToString();
                hb.AddChild(lblDate);
                ct = (Control) calendarbutonClass.New();
                //ct.Connect("date_selected", lblDate, ("SetText"));
                slotColor = Colors.LightGreen;
                break;

            default:
                ct = new LineEdit();
                slotColor = Colors.Black;
                break;
        }

        hb.AddChild(ct);
        toNode.AddChild(hb);
        toNode.SetSlot(hb.GetIndex(), true, 0, slotColor, false, 0, Colors.Red, null, null);
    }
}
