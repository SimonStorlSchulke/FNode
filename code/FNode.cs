using System.Collections.Generic;
using Godot;
using System.Linq;

public abstract class FNode : GraphNode {

    public Dictionary<string, FInput> inputs = new Dictionary<string, FInput>();
    public Dictionary<string, FOutput> outputs = new Dictionary<string, FOutput>();
    public string category = "other";

    public override void _Ready() {
        Name = "FNode_" + GetParent().GetChildCount();
        ShowClose = true;
        Title = UIUtil.SnakeCaseToWords(this.GetType().Name.Replace("FNode", ""));
        this.RectMinSize = new Vector2(250, 0);
        UIUtil.CreateUI(this);
        Connect(
            "close_request", 
            GetParent(), 
            nameof(NodeTree.DeleteNode), 
            new Godot.Collections.Array(){this});
    }

    public bool ExecutiveMethodRan = false;
    // If a Node isExecutiveNode, this Method will always be called regardless of connected Nodes.
    public virtual void ExecutiveMethod(){
        ExecutiveMethodRan = true;
    }

    
    public bool HasExecutiveMethod() {
        return this.GetType().GetMethod("ExecutiveMethod").DeclaringType == this.GetType();
    }

    static int indexerNum = 0;
    public static int IdxNext() {
        int val = indexerNum;
        indexerNum++;
        return val;
    }
    public static void IdxReset(int num = 0) {
        indexerNum = num;
    }

    public enum FNodeSlotTypes {
        FILE,
        STRING,
        BOOL,
        INT,
        FLOAT,
        DATE
    }

    
    public void ChangeSlotType(FInput finput, FNodeSlotTypes toType) {
        int childIdx = outputs.Count + finput.idx;
        Node n = GetChild(childIdx);
        string labelText = n.GetChild<Label>(0).Text;
        RemoveChild(n);
        n.QueueFree();
        int idx = finput.idx;
        var connectedSlot = finput.connectedTo;

        switch(toType) {
            case FNodeSlotTypes.FILE:
                finput = new FInputFile(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.STRING:
                finput = new FInputString(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.BOOL:
                finput = new FInputBool(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.INT:
                finput = new FInputInt(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.FLOAT:
                finput = new FInputFloat(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.DATE:
                finput = new FInputDate(this, idx);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
        }
        finput.connectedTo = connectedSlot;
        inputs[inputs.ElementAt(finput.idx).Key] = finput;
    }


    public void ChangeSlotType(FOutput foutput, GetOutputValue method, FNodeSlotTypes toType) {
        int childIdx = foutput.idx;
        Node n = GetChild(childIdx);
        string labelText = n.GetChild<Label>(1).Text;
        RemoveChild(n);
        n.QueueFree();
        int idx = foutput.idx;
        var oldConnectionList = foutput.ConnectedTo();

        switch(toType) {
            case FNodeSlotTypes.FILE:
                foutput = new FOutputFile(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.STRING:
                foutput = new FOutputString(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.BOOL:
                foutput = new FOutputBool(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.INT:
                foutput = new FOutputInt(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.FLOAT:
                foutput = new FOutputFloat(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.DATE:
                foutput = new FOutputDate(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
        }
        foreach (var conSlot in oldConnectionList) {
            conSlot.connectedTo = foutput; //TODO Test if this is necessary
        }

        outputs[outputs.ElementAt(foutput.idx).Key] = foutput;
    }

    public virtual Godot.Collections.Dictionary<string, object> Serialize() {
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>() {
        };

        saveData.Add("Type", GetType().ToString());
        saveData.Add("NodeName", Name);
        saveData.Add("OffsetX", Offset.x);
        saveData.Add("OffsetY", Offset.y);

        Godot.Collections.Dictionary<string, object> saveDatInputs = new Godot.Collections.Dictionary<string, object>() {
        };

        foreach (var input in inputs) {
            saveDatInputs.Add(input.Key, input.Value.GetDefaultValueFromUI());
        }

        saveData.Add("Inputs", saveDatInputs);

        return saveData;
    }
}

