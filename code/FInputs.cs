using System.IO;
using System;
using System.Collections.Generic;
using Godot;
using System.Linq;


public abstract class FInput {
    protected object defaultValue;
    Control GetUI;
    public FNode owner;
    public FOutput connectedTo;
    public int idx;

    public FInput(FNode owner, int idx=-1) {
        this.owner = owner;
        this.idx = idx == -1 ? this.idx = FNode.IdxNext() : idx;
    }

    protected object AutoSlotConversion(object value) {
        GD.Print(value.GetType());
        if (this.GetType() == typeof(FInputString) && value.GetType() == typeof(DateTime)) {
            return ((DateTime)value).ToString();
        }
        else if (this.GetType() == typeof(FInputString) && value.GetType() == typeof(int)) {
            return ((int)value).ToString();
        }
        else if (this.GetType() == typeof(FInputString) && value.GetType() == typeof(float)) {
            return ((float)value).ToString("0.00");
        }
        else if (this.GetType() == typeof(FInputString) && value.GetType() == typeof(FileInfo)) {
            return ((FileInfo)value).FullName;
        }
        else if (this.GetType() == typeof(FInputString) && value.GetType() == typeof(bool)) {
            return ((bool)value).ToString();
        }
        else return value;
    }

    public virtual object Get() {
        if (connectedTo != null) {
            return AutoSlotConversion(connectedTo.Get());
        } else {
            UpdateDefaultValueFromUI();
            return defaultValue;
        }
    }

    public abstract void UpdateDefaultValueFromUI();
}

public class FInputString : FInput {
    public FInputString(FNode owner, int idx=-1) : base(owner, idx) {
    }

    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as LineEdit).Text.Replace("[LINEBREAK]", "\n");
    }
}

public class FInputFile : FInput {
    public FInputFile(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = new System.IO.FileInfo((nd as LineEdit).Text);
    }
}

public class FInputInt : FInput {
    public FInputInt(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as SpinBox).Value;
    }
}

public class FInputFloat : FInput {
    public FInputFloat(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as SpinBox).Value;
    }
}

public class FInputBool : FInput {
    public FInputBool(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (nd as CheckBox).Pressed;
    }
}

public class FInputDate : FInput {
    public FInputDate(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = DateTime.Parse((nd as Label).Text);
        GD.Print(defaultValue);
    }
}

