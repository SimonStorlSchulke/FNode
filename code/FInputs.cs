using System;
using System.Collections.Generic;
using Godot;
using System.Linq;


public abstract class FInput {
    protected object defaultValue;
    Control GetUI;
    public FNode owner;
    public int idx;

    public FInput(FNode owner, int idx) {
        this.owner = owner;
        this.idx = idx;
    }

    public object Get() {
        FOutput cTo = ConnectedTo();
        if (cTo != null) {
            return cTo.Get();
        } else {
            UpdateDefaultValueFromUI();
            return defaultValue;
        }
    }

    public FOutput ConnectedTo() {
        NodeTree nt = owner.GetParent<NodeTree>();
        Godot.Collections.Array links = nt.GetConnectionList();
        
        // There must be a better way...
        foreach (Godot.Collections.Dictionary link in links) {
            if ((string)link["to"] == owner.Name && (int)link["to_port"] == idx) {
                return nt.GetNode<FNode>((string)link["from"]).outputs.ElementAt((int)link["from_port"]).Value;
            }
        }
        return null;
    }

    public abstract void UpdateDefaultValueFromUI();
}

public class FInputString : FInput {
    public FInputString(FNode owner, int idx) : base(owner, idx) {
    }

    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as LineEdit).Text;
    }
}

public class FInputFile : FInput {
    public FInputFile(FNode owner, int idx) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as LineEdit).Text;
    }
}

public class FInputInt : FInput {
    public FInputInt(FNode owner, int idx) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as SpinBox).Value;
    }
}

public class FInputFloat : FInput {
    public FInputFloat(FNode owner, int idx) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as SpinBox).Value;
    }
}

public class FInputBool : FInput {
    public FInputBool(FNode owner, int idx) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as CheckBox).Pressed;
    }
}

public class FInputDate : FInput {
    public FInputDate(FNode owner, int idx) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = DateTime.Now;
    }
}

