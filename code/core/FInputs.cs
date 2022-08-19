using System.IO;
using System;
using System.Collections.Generic;
using Godot;

public enum SlotType {
    FILE,
    STRING,
    BOOL,
    INT,
    FLOAT,
    DATE,
    IMAGE,
    LIST,
    OTHER
}

public class FInput {
    public object DefaultValue;
    public object initialValue;
    Control GetUI;
    public FNode owner;
    public FOutput connectedTo;
    public int idx;
    public string description;
    public SlotType slotType = SlotType.OTHER;

    public FInput(FNode owner, int idx=-1, string description="", object initialValue=null) {
        this.owner = owner;
        this.description = description;
        this.initialValue = initialValue;
        this.idx = idx == -1 ? this.idx = FNode.IdxNext() : idx;
    }

    protected delegate object SlotConversion(object outputVal, FInput inputType);

    ///<summary> Dictionary of delegates handling conversions between different data (slot) types </summary>
    protected static Dictionary<Tuple<Type, Type>, SlotConversion> slotConversionsDict = new Dictionary<Tuple<Type, Type>, SlotConversion>() {


        {new Tuple<Type, Type>(typeof(FileInfo), typeof(FInputImage)), delegate(object outVal, FInput fIn) {
        try {
            return new ImageMagick.MagickImage(((FileInfo)outVal).FullName);
        } catch {
            return null;
        }
        }},

        {new Tuple<Type, Type>(typeof(string), typeof(FInputImage)), delegate(object outVal, FInput fIn) {
        try {
            return new ImageMagick.MagickImage((string)outVal);
        } catch {
            return null;
        }
        }},

        #region fileInput conversions
        {new Tuple<Type, Type>(typeof(string), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            if (FileUtil.IsAbsolutePath(((string)outVal))) {
                return new FileInfo((string)outVal); //Only allow absolute paths
            }
            return null;
        }},
        {new Tuple<Type, Type>(typeof(bool), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            return null; //nonsensical
        }},
        {new Tuple<Type, Type>(typeof(int), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            return null; //nonsensical
        }},
        {new Tuple<Type, Type>(typeof(float), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            return null; //nonsensical
        }},
        {new Tuple<Type, Type>(typeof(DateTime), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            return null; //nonsensical
        }},
        {new Tuple<Type, Type>(typeof(Godot.Collections.Array), typeof(FInputFile)), delegate(object outVal, FInput fIn) {
            return null; //nonsensical
        }},
        #endregion

        #region stringInput conversions
        {new Tuple<Type, Type>(typeof(FileInfo), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return ((FileInfo)outVal).FullName;
        }},
        {new Tuple<Type, Type>(typeof(bool), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return (bool)outVal ? "True" : "False";
        }},
        {new Tuple<Type, Type>(typeof(int), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return ((int)outVal).ToString();
        }},
        {new Tuple<Type, Type>(typeof(float), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return ((float)outVal).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }},
        {new Tuple<Type, Type>(typeof(DateTime), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return ((DateTime)outVal).ToString();
        }},
        {new Tuple<Type, Type>(typeof(Godot.Collections.Array), typeof(FInputString)), delegate(object outVal, FInput fIn) {
            return ((Godot.Collections.Array)outVal).ToString();
        }},
        #endregion

        #region boolInput conversions
        {new Tuple<Type, Type>(typeof(FileInfo), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
            return System.IO.File.Exists(((FileInfo)outVal).FullName);
        }},
        {new Tuple<Type, Type>(typeof(string), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
            return (string)outVal != "";
        }},
        {new Tuple<Type, Type>(typeof(int), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
               return (int)outVal > 0;
        }},
        {new Tuple<Type, Type>(typeof(float), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
            return (float)outVal >= 1f;
        }},
        {new Tuple<Type, Type>(typeof(DateTime), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
            return false;
        }},
        {new Tuple<Type, Type>(typeof(Godot.Collections.Array), typeof(FInputBool)), delegate(object outVal, FInput fIn) {
            return ((Godot.Collections.Array)outVal).Count > 0;
        }},
        #endregion

        #region intInput conversions
        {new Tuple<Type, Type>(typeof(FileInfo), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            return System.IO.File.Exists(((FileInfo)outVal).FullName) ? 1 : 0;
        }},
        {new Tuple<Type, Type>(typeof(string), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            try {
                return System.Convert.ToInt32((string)outVal);
            } catch {
                return 0;
            }
        }},
        {new Tuple<Type, Type>(typeof(bool), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            return (bool)outVal ? 1 : 0;
        }},
        {new Tuple<Type, Type>(typeof(float), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            return (int)Mathf.Round(((float)outVal));
        }},
        {new Tuple<Type, Type>(typeof(DateTime), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            //nonsensical so return 0 - needs a converter Node to get days / years...
            return 0;
        }},
        {new Tuple<Type, Type>(typeof(Godot.Collections.Array), typeof(FInputInt)), delegate(object outVal, FInput fIn) {
            return ((Godot.Collections.Array)outVal).Count;
        }},
        #endregion

        #region floatInput conversions
        {new Tuple<Type, Type>(typeof(FileInfo), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            return System.IO.File.Exists(((FileInfo)outVal).FullName) ? 1f : 0f;
        }},
        {new Tuple<Type, Type>(typeof(string), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            try {
                return System.Convert.ToSingle((string)outVal);
            } catch {
                return 0f;
            }
        }},
        {new Tuple<Type, Type>(typeof(bool), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            return (bool)outVal ? 1f : 0f;
        }},
        {new Tuple<Type, Type>(typeof(int), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            return (float)((int)outVal);
        }},
        {new Tuple<Type, Type>(typeof(DateTime), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            //nonsensical so return 0 - needs a converter Node to get days / years...
            return 0f;
        }},
        {new Tuple<Type, Type>(typeof(Godot.Collections.Array), typeof(FInputFloat)), delegate(object outVal, FInput fIn) {
            return (float)((Godot.Collections.Array)outVal).Count;
        }},
        #endregion
    };

    ///<summary> uses the slotConversionsDict to convert incoming slot-connections that aren't the same type as the input </summar>
    protected object AutoSlotConversion(object value) {
        var slotType = this.GetType();

        if (value == null && slotType == typeof(FInputBool)) {
            return false;
        } else if (value == null) {
            return null;
        }

        var valueType = value.GetType();

        
        bool invalidImageConversion = valueType != typeof(ImageMagick.MagickImage) && slotType == typeof(FInputImage) || valueType == typeof(ImageMagick.MagickImage) && slotType != typeof(FInputImage);
        
        if (valueType == typeof(FileInfo) || valueType != typeof(string) || valueType != typeof(ImageMagick.MagickImage)) {
            invalidImageConversion = false;
        }

        if (invalidImageConversion) {
            return null;
        }
        
        if (valueType == typeof(ImageMagick.MagickImage) && slotType != typeof(FInputImage)) {
            return null;
        }

        if (valueType == typeof(System.Double)) {
            value = (float)value;
        }

        if (slotType == typeof(FInputList) && valueType != typeof(Godot.Collections.Array)) {
            return new Godot.Collections.Array() {value};
        }


        try {
            return slotConversionsDict[new Tuple<Type, Type>(valueType, slotType)](value, this);
        } catch {
            return value; //dangerous..
        }
    }

    ///<summary> Get the input value by either parsing the connected slot or using the user-entered defaultvalue if no slot is connected </summary>
    public virtual T Get<T>() {
        if (connectedTo != null) {
            return (T)AutoSlotConversion(connectedTo.Get());
        } else {
            UpdateDefaultValueFromUI();
            return (T)AutoSlotConversion(DefaultValue); //TODO - maybe not necessary
        }
    }

    ///<summary> Updates the Nodes defaultValue parameter and sanitizes it </summary> 
    public virtual void UpdateDefaultValueFromUI(){}
    
    ///<summary> Returns the actual text etc the User has entered in the Input FIeld </summary> 
    public virtual object GetDefaultValueFromUI(){return null;}
    
    ///<summary> Returns the actual text etc the User has entered in the Input FIeld </summary> 
    public virtual void UpdateUIFromValue(object value){}
}

public class FInputFile : FInput {
    public FInputFile(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.FILE;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        string path = (nd as LineEdit).Text;
        if (FileUtil.IsAbsolutePath(path))
            DefaultValue = new System.IO.FileInfo(path);
        else
            DefaultValue = null;
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return (nd as LineEdit).Text;
    }

    public override void UpdateUIFromValue(object value) {
        ((LineEdit)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Text = (string)value;
    }
}

public class FInputString : FInput {
    public FInputString(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.STRING;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(2);
        DefaultValue = (nd as LineEdit).Text.Replace("[LINEBREAK]", "\n");
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(2);
        return (nd as LineEdit).Text;
    }

    public override void UpdateUIFromValue(object value) {
        ((LineEdit)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(2)).Text = (string)value;
    }
}

public class FInputList : FInput {
    public FInputList(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.LIST;
        this.DefaultValue = initialValue == null ? new Godot.Collections.Array() : initialValue;
    }

    public override void UpdateDefaultValueFromUI() {
        //Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return DefaultValue;
    }

    public override void UpdateUIFromValue(object value) {
        ((LineEdit)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Text = (string)value;
    }
}


public class FInputInt : FInput {
    public int min;
    public int max;
    public FInputInt(FNode owner, int idx = -1, string description = "", object initialValue = null, int min = int.MinValue, int max = int.MaxValue) : base(owner, idx, description, initialValue) {
        this.min = min;
        this.max = max;
        slotType = SlotType.INT;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        DefaultValue = (int)(nd as SpinBox).Value;
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return (int)(nd as SpinBox).Value;
    }

    public override void UpdateUIFromValue(object value) {
        ((SpinBox)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Value = (float)value;
    }
}

public class FInputFloat : FInput {
    public float min;
    public float max;
    public FInputFloat(FNode owner, int idx = -1, string description = "", object initialValue = null, float min = -Mathf.Inf, float max = Mathf.Inf) : base(owner, idx, description, initialValue) {
        this.min = min;
        this.max = max;
        slotType = SlotType.FLOAT;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        DefaultValue = (float)(nd as SpinBox).Value;
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return (float)(nd as SpinBox).Value;
    }

    public override void UpdateUIFromValue(object value) {
        ((SpinBox)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Value = (float)value;
    }
}

public class FInputBool : FInput {
    public FInputBool(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.BOOL;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        DefaultValue = (nd as CheckBox).Pressed;
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return (nd as CheckBox).Pressed;
    }

    public override void UpdateUIFromValue(object value) {
        ((CheckBox)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Pressed = (bool)value;
    }
}

public class FInputDate : FInput {
    public FInputDate(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.DATE;
    }

    public override void UpdateDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        DefaultValue = DateTime.Parse((nd as Label).Text);
    }

    public override object GetDefaultValueFromUI() {
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return (nd as Label).Text; // kinda wonky...
    }

    public override void UpdateUIFromValue(object value) {
        ((Label)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Text = (string)value;
    }
}


public class FInputImage : FInput {
    public FInputImage(FNode owner, int idx = -1, string description = "", object initialValue = null) : base(owner, idx, description, initialValue) {
        slotType = SlotType.IMAGE;
    }

    public override void UpdateDefaultValueFromUI() {
        /*
        Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        string path = (nd as LineEdit).Text;
        if (FileUtil.IsAbsolutePath(path))
            defaultValue = path;
        else
            defaultValue = null;
            */
    }

    public override object GetDefaultValueFromUI() {
        //Node nd = owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1);
        return null;
    }

    public override void UpdateUIFromValue(object value) {
        //((LineEdit)owner.GetChild<HBoxContainer>(owner.outputs.Count + idx).GetChild(1)).Text = (string)value;
    }
}

