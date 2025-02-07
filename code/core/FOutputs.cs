using System.Collections.Generic;
using Godot;
using System.Linq;


public delegate object GetOutputValue();

public partial class FOutput {
    public FNode owner;
    public GetOutputValue Get;
    public int idx;
    public string description;
    public SlotType slotType = SlotType.OTHER;

    public FOutput(FNode owner, GetOutputValue method, int idx=-1, string description = "") {
        this.owner = owner;
        this.description = description;
        this.idx = idx == -1 ? this.idx = FNode.IdxNext() : idx;
        Get = method;
    }


    public List<FInput> GetConnectedInputs() {
        NodeTree nt = owner.GetParent<NodeTree>();
        Godot.Collections.Array<Godot.Collections.Dictionary> links = nt.GetConnectionList();
        List<FInput> inpList = new List<FInput>();
        
        // There must be a better way...
        foreach (Godot.Collections.Dictionary link in links) { //TODO migration - check. won't work probably
            if ((string)link["from"] == owner.Name && (int)link["from_port"] == idx) {
                inpList.Add(nt.GetNode<FNode>((string)link["to"]).inputs.ElementAt((int)link["to_port"]).Value);
            }
        }
        return inpList;
    }

}


public partial class FOutputInt : FOutput {
    public FOutputInt(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.INT;
    }
}


public partial class FOutputFloat : FOutput {
    public FOutputFloat(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.FLOAT;
    }
}


public partial class FOutputBool : FOutput {
    public FOutputBool(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.BOOL;
    }
}


public partial class FOutputString : FOutput {
    public FOutputString(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.STRING;
    }
}


public partial class FOutputList : FOutput {
    public FOutputList(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.LIST;
    }
}


public partial class FOutputFile : FOutput {
    public FOutputFile(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.FILE;
    }
}


public partial class FOutputImage : FOutput {
    public FOutputImage(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.FILE;
    }
}


public partial class FOutputColor : FOutput {
    public FOutputColor(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.COLOR;
    }
}


public partial class FOutputDate : FOutput {
    public FOutputDate(FNode owner, GetOutputValue method, int idx=-1, string description = "") : base(owner, method, idx, description) {
        slotType = SlotType.DATE;
    }
}