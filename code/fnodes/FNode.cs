using System.Collections.Generic;
using Godot;
using System.Linq;
using System;

public abstract class FNode : GraphNode {

    public Dictionary<string, FInput> inputs = new Dictionary<string, FInput>();
    public Dictionary<string, FOutput> outputs = new Dictionary<string, FOutput>();
    public string category = "other";
    public const string RunBeforeEvaluationGroup = "run_before_evaluation_group";
    public const string RunBeforeIterationGroup = "run_before_iteration_group";
    public const string AwaiterNodesGroup = "awaiter_nodes_group";

    public override void _EnterTree() {
        Name = "FNode_" + Guid.NewGuid();
    }

    public override void _Ready() {
        ShowClose = true;
        Title = UIUtil.SnakeCaseToWords(this.GetType().Name.Replace("FNode", ""));
        this.RectMinSize = new Vector2(250, 0);
        UIUtil.CreateUI(this);
        Connect(
            "close_request", 
            GetParent(),
            nameof(NodeTree.DeleteNode), 
            new Godot.Collections.Array(){this});

        if (this.GetType().GetMethod(nameof(OnBeforeEvaluation)).DeclaringType == this.GetType()) {
            AddToGroup(RunBeforeEvaluationGroup);
        }
        if (this.GetType().GetMethod(nameof(OnNextIteration)).DeclaringType == this.GetType()) {
            AddToGroup(RunBeforeIterationGroup);
        }
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
        DATE,
        LIST,
        IMAGE
    }
    
    public void AddOptionEnum(string name, string[] options, string callbackMethod="", string description="") {
        
        OptionButton ob = new OptionButton();
        foreach (var item in options) {
            ob.AddItem(item);
        }
        HbOption hb = new HbOption(name, ob);
        hb.HintTooltip = description;
        hb.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        ob.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        
        AddChild(hb);
        if (callbackMethod != "") {
            ob.Connect("item_selected", this, callbackMethod);
        }
    }

    public string GetSelectedOption(string optionButtonName) {
        OptionButton btn = GetNode<HbOption>(optionButtonName).optionButton;
        return btn.GetItemText(btn.Selected);
    }

    
    public void ChangeSlotType(FInput finput, FNodeSlotTypes toType, object initialValue = null) {
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
                finput = new FInputString(this, idx, initialValue: initialValue);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.BOOL:
                finput = new FInputBool(this, idx, initialValue: initialValue);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.INT:
                finput = new FInputInt(this, idx, initialValue: initialValue);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.FLOAT:
                finput = new FInputFloat(this, idx, initialValue: initialValue);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.DATE:
                finput = new FInputDate(this, idx, initialValue: initialValue);
                UIUtil.AddInputUI(this, labelText, finput, childIdx);
                break;
            case FNodeSlotTypes.LIST:
                finput = new FInputList(this, idx, initialValue: initialValue);
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
        var oldConnectionList = foutput.GetConnectedInputs();

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
            case FNodeSlotTypes.LIST:
                foutput = new FOutputList(this, method, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
        }
        foreach (var conSlot in oldConnectionList) {
            conSlot.connectedTo = foutput; //TODO Test if this is necessary
        }

        outputs[outputs.ElementAt(foutput.idx).Key] = foutput;
    }

    // Dangerous - change output type without changing it's Get-Method
    public void ChangeSlotType(FOutput foutput, FNodeSlotTypes toType) {
        int childIdx = foutput.idx;
        Node n = GetChild(childIdx);
        string labelText = n.GetChild<Label>(1).Text;
        RemoveChild(n);
        n.QueueFree();
        GetOutputValue m = foutput.Get;
        int idx = foutput.idx;
        var oldConnectionList = foutput.GetConnectedInputs();

        switch(toType) {
            case FNodeSlotTypes.FILE:
                foutput = new FOutputFile(this, m, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.STRING:
                foutput = new FOutputString(this, m, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.BOOL:
                foutput = new FOutputBool(this, m, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.INT:
                foutput = new FOutputInt(this, m, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.FLOAT:
                foutput = new FOutputFloat(this, m, idx);
                UIUtil.AddOutputUI(this, labelText, foutput, childIdx);
                break;
            case FNodeSlotTypes.DATE:
                foutput = new FOutputDate(this, m, idx);
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

            // ugly hack to differentiate between ints and float in savefile-lists: If value is an Int, save as string "%FNODE-INT%value" instead (example: %FNODE-INT%10)
            if (input.Value is FInputList) {

                var originalSaveArr = ((Godot.Collections.Array)input.Value.DefaultValue);
                var saveArr = ((Godot.Collections.Array)input.Value.DefaultValue).Duplicate();

                for (int i = 0; i < saveArr.Count; i++) {
                    if (originalSaveArr[i] is System.Int32) {
                        saveArr[i] = "%FNODE-INT%"+saveArr[i];
                    }
                }
                saveDatInputs.Add(input.Key, saveArr);
            } 
            else {
                saveDatInputs.Add(input.Key, input.Value.GetDefaultValueFromUI());
            }
        }

        saveData.Add("Inputs", saveDatInputs);

        Godot.Collections.Dictionary<string, object> saveDatOptionButtons = new Godot.Collections.Dictionary<string, object>() {
        };

        if (GetChildCount() > inputs.Count + outputs.Count) {
            foreach (var item in GetChildren()) {
                if (!(item is HbInput || item is HbOutput)) {
                    switch(item) {
                        case HbOption hbOpt:
                            saveDatOptionButtons.Add(
                                hbOpt.Name, 
                                hbOpt.optionButton.Selected);
                            break;
                    }
                }
            }
        }

        saveData.Add("OptionButtons", saveDatOptionButtons);

        return saveData;
    }

    public static void Deserialize(Godot.Collections.Dictionary nodeData, Project pr) {
        string nodeType = (string)nodeData["Type"];
        string nodeName = (string)nodeData["NodeName"];
        Vector2 offset = new Vector2((float)nodeData["OffsetX"], (float)nodeData["OffsetY"]);
        FNode fn = pr.NodeTree.OnAddNode(nodeType, offset, nodeName);
        //fn.Name = nodeName;

        if (fn is IFNodeVarInputSize) {
            (fn as IFNodeVarInputSize).SetInputSize(((Godot.Collections.Dictionary)nodeData["Inputs"]).Count);
        }

        int i = 0;
        foreach (System.Collections.DictionaryEntry inputData in (Godot.Collections.Dictionary)nodeData["Inputs"]) {

            if (inputData.Value == null) {
                continue;
            }

            // ugly hack #2 - see comment in Serialize()
            if(inputData.Value.GetType() == typeof(Godot.Collections.Array)) {

                var loadAr = inputData.Value as Godot.Collections.Array;

                for (int j = 0; j < loadAr.Count; j++) {
                    if (loadAr[j] is System.String) {
                        string str = (string)loadAr[j];
                        if(str.StartsWith("%FNODE-INT%")) {
                            GD.Print(str.Substring(11));
                            loadAr[j] = System.Convert.ToInt32(str.Substring(11));
                        }
                    }
                }

                fn.inputs[(string)inputData.Key].DefaultValue = loadAr;
            } else {
                fn.inputs[(string)inputData.Key].UpdateUIFromValue(inputData.Value);
            }
            i++;
        }

         foreach (System.Collections.DictionaryEntry optionButtonData in (Godot.Collections.Dictionary)nodeData["OptionButtons"]) {
            try {
                fn.GetNode<HbOption>((string)optionButtonData.Key).optionButton.Selected = System.Convert.ToInt32(optionButtonData.Value);
            } catch (System.Exception e) {
                Errorlog.Log(fn, $"failed loading Optionbutton {optionButtonData.Key} {e}");
            }
         }
    }

    ///<summary>This will be called before each NodeTree iteration (optimized via group CallGroup)</summary>
    public virtual void OnNextIteration(){}
    public virtual void OnBeforeEvaluation(){}
}

