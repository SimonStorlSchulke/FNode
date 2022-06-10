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
        // TODO this Could probably be more effective
        Type slotType = this.GetType();
        Type valueType = value.GetType();
        if (slotType == typeof(FInputString) && valueType == typeof(DateTime)) {
            return ((DateTime)value).ToString();
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(int)) {
            return ((int)value).ToString();
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(float)) {
            return ((float)value).ToString("0.00");
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(double)) {
            return ((double)value).ToString("0.00");
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(FileInfo)) {
            return ((FileInfo)value).FullName;
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(bool)) {
            return ((bool)value).ToString();
        }
        else if (slotType == typeof(FInputInt) && valueType == typeof(float)) {
            return (int)((float)value);
        }
        else if (slotType == typeof(FInputFloat) && valueType == typeof(int)) {
            return (float)((int)value);
        }
        else if (slotType == typeof(FInputBool) && valueType == typeof(int)) {
            return ((int)value) > 0 ? true : false;
        }
        else if (slotType == typeof(FInputBool) && valueType == typeof(float)) {
            return ((float)value) >= 1f ? true : false;
        }
        else if (slotType == typeof(FInputFloat) && valueType == typeof(bool)) {
            return (bool)value ? 1.0f : 0.0f;
        }
        else if (slotType == typeof(FInputInt) && valueType == typeof(bool)) {
            return (bool)value ? 1 : 0;
        }
        else if (slotType == typeof(FInputString) && valueType == typeof(bool)) {
            return (bool)value ? "True" : "False";
        }
        else return value;
    }

    public virtual object Get() {
        if (connectedTo != null) {
            return AutoSlotConversion(connectedTo.Get());
        } else {
            UpdateDefaultValueFromUI();
            return AutoSlotConversion(defaultValue);
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
        defaultValue = (int)(nd as SpinBox).Value;
    }
}

public class FInputFloat : FInput {
    public FInputFloat(FNode owner, int idx=-1) : base(owner, idx) {
    }
    
    public override void UpdateDefaultValueFromUI()
    {
        GD.Print(owner.GetChild<HBoxContainer>(owner.outputs.Count + idx));
        GD.Print(owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1));
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        defaultValue = (float)(nd as SpinBox).Value;
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
    }
}

