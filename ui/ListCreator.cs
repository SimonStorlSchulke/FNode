using Godot;
using System;

public class ListCreator : AcceptDialog {

    static Godot.Collections.Array list;

    [Export] NodePath NPTypeSelector;
    OptionButton obTypeSelector;

    [Export] NodePath NPItemsBox;
    VBoxContainer itemsBox;

    [Export] NodePath NPItemCount;
    Label lblItemCount;
    public static ListCreator inst;
    public static FInputList connectedListInput; //TODO hmmm... how? FNip can't be passed by signal..

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        inst = this;
        obTypeSelector = GetNode<OptionButton>(NPTypeSelector);
        itemsBox = GetNode<VBoxContainer>(NPItemsBox);
        lblItemCount = GetNode<Label>(NPItemCount);
    }

    public void ShowCreator(Godot.Collections.Array listInput, string connectedNodeName, string connectedSlotName) {
        list = listInput;
        //GD.Print($"{connectedNodeName} slotname: {connectedSlotName}");
        connectedListInput = Main.inst.currentProject.NodeTree.GetNode<FNode>(connectedNodeName).inputs[connectedSlotName] as FInputList; //Dangerous...
        if (connectedListInput.defaultValue == null) {
            GD.Print(1);
            connectedListInput.defaultValue = new Godot.Collections.Array();
        }
        for (int i = 0; i < ((Godot.Collections.Array)connectedListInput.defaultValue).Count; i++) {
            AddItem(((Godot.Collections.Array)connectedListInput.defaultValue)[i]);
        }
        inst.PopupCentered();
    }

    public void OnPopupHide() {
        connectedListInput.defaultValue = new Godot.Collections.Array();
        foreach (Node item in itemsBox.GetChildren()) {
            Node itemCtl = item.GetChild(1);
            string itemType = (string)item.GetGroups()[0];
            object itemValue;
            switch (itemType) {
                case "file":
                    string path = ((LineEdit)itemCtl).Text;
                    if (FileUtil.IsAbsolutePath(path))
                        itemValue = new System.IO.FileInfo(path).FullName;
                    else
                        itemValue = null;
                    break;
                case "text":
                    itemValue = ((LineEdit)itemCtl).Text;
                    break;
                case "bool":
                    itemValue = ((CheckBox)itemCtl).Pressed;
                    break;
                case "int":
                    itemValue = (int)((SpinBox)itemCtl).Value;
                    break;
                case "float":
                    itemValue = ((SpinBox)itemCtl).Value;
                    break;
                case "date":
                    itemValue = ((SpinBox)itemCtl).Value;
                    break;
                default:
                    itemValue = ((SpinBox)itemCtl).Value;
                    break;
            }
            ((Godot.Collections.Array)connectedListInput.defaultValue).Add(itemValue);
            item.QueueFree();
        }
    }

    static System.Collections.Generic.Dictionary<string, string> TypenamesUI 
        = new System.Collections.Generic.Dictionary<string, string>(){
            {"System.String", "text"},
            {"System.Boolean", "bool"},
            {"System.Int32", "int"},
            {"System.Single", "float"},
        };

    public void AddItem(string type) {
        HBoxContainer hbItem = new HBoxContainer();
        Label lblItem = new Label();
        lblItem.Text = type;
        lblItem.RectMinSize = new Vector2(90, 0);
        hbItem.AddChild(lblItem);

        switch (type) {
            case "file":
                hbItem.AddChild(new LineEdit());
                break;
            case "text":
                hbItem.AddChild(new LineEdit());
                break;
            case "bool":
                hbItem.AddChild(new CheckBox());
                break;
            case "int":
                SpinBox spI = new SpinBox();
                spI.MinValue = -Mathf.Inf;
                hbItem.AddChild(spI);
                break;
            case "float":
                SpinBox spF = new SpinBox();
                spF.MinValue = -Mathf.Inf;
                spF.Step = 0.01;
                hbItem.AddChild(spF);
                break;
            case "date":
                SpinBox spT = new SpinBox(); //TODO
                hbItem.AddChild(spT);
                break;
            default:
                break;
            }
            Button btnRemoveItem = new Button();
            btnRemoveItem.Text = " X ";
            hbItem.AddChild(btnRemoveItem);
            hbItem.AddToGroup(type);
            hbItem.GetChild<Control>(1).RectMinSize = new Vector2(200, 0);
            itemsBox.AddChild(hbItem);
            btnRemoveItem.Connect("pressed", this, nameof(RemoveItem), new Godot.Collections.Array{hbItem});
            lblItemCount.Text = itemsBox.GetChildCount().ToString() + " Items";
    }

    //ugly duplicated code but connections don't allow optional function parameters
    public void AddItem(object value) {
        HBoxContainer hbItem = new HBoxContainer();
        Label lblItem = new Label();
        lblItem.RectMinSize = new Vector2(90, 0);
        if (value == null) {
            return;
        }
        try {
            lblItem.Text = TypenamesUI[value.GetType().ToString()];
        } catch (System.Exception e) {
            lblItem.Text = "unknown type";
            Errorlog.Log(this, e);
        }
        hbItem.AddChild(lblItem);

        switch (value) {
            case System.IO.FileInfo fi:
            LineEdit leF = new LineEdit();
                if (fi != null) {
                    leF.Text = fi.FullName;
                }
                hbItem.AddToGroup("file");
                hbItem.AddChild(leF);
                break;
            case string fi:
            LineEdit leT = new LineEdit();
                leT.Text = (string)value;
                hbItem.AddToGroup("text");
                hbItem.AddChild(leT);
                break;
            case bool fi:
                CheckBox cb = new CheckBox();
                cb.Pressed = (bool)value;
                hbItem.AddToGroup("bool");
                hbItem.AddChild(cb);
                break;
            case int fi:
                SpinBox spI = new SpinBox();
                spI.Value = (int)value;
                spI.MinValue = -Mathf.Inf;
                hbItem.AddToGroup("int");
                hbItem.AddChild(spI);
                break;
            case float fi:
                SpinBox spF = new SpinBox();
                spF.MinValue = -Mathf.Inf;
                spF.Step = 0.01;
                spF.Value = (float)value;
                hbItem.AddToGroup("float");
                hbItem.AddChild(spF);
                break;
            case DateTime fi:
                SpinBox spT = new SpinBox(); //TODO
                hbItem.AddToGroup("date");
                hbItem.AddChild(spT);
                break;
            default:
                break;
            }
            Button btnRemoveItem = new Button();
            btnRemoveItem.Text = " X ";
            hbItem.AddChild(btnRemoveItem);
            hbItem.GetChild<Control>(1).RectMinSize = new Vector2(200, 0);
            itemsBox.AddChild(hbItem);
            btnRemoveItem.Connect("pressed", this, nameof(RemoveItem), new Godot.Collections.Array{hbItem});
            lblItemCount.Text = itemsBox.GetChildCount().ToString() + " Items";
    }

    public void RemoveItem(Node nd) {
        nd.QueueFree();
    }
}
