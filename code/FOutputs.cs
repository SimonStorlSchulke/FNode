using System.Collections.Generic;
using Godot;
using System.Linq;


public delegate object GetOutputValue();

public class FOutput {
    public FNode owner;
    public GetOutputValue Get;
    public int idx;
    public FOutput(FNode owner, int idx, GetOutputValue method) {
        this.owner = owner;
        this.idx = idx;
        Get = method;
    }

    public FInput ConnectedTo() {
        NodeTree nt = owner.GetParent<NodeTree>();
        Godot.Collections.Array links = nt.GetConnectionList();
        
        // There must be a better way...
        foreach (Godot.Collections.Dictionary link in links) {
            if ((string)link["from"] == owner.Name && (int)link["from_port"] == idx) {
                return nt.GetNode<FNode>((string)link["to"]).inputs.ElementAt((int)link["to_port"]).Value;
            }
        }
        return null;
    }

}

public class FOutputInt : FOutput {
    public FOutputInt(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}

public class FOutputFloat : FOutput {
    public FOutputFloat(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}

public class FOutputBool : FOutput {
    public FOutputBool(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}

public class FOutputString : FOutput {
    public FOutputString(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}

public class FOutputFile : FOutput {
    public FOutputFile(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}

public class FOutputDate : FOutput {
    public FOutputDate(FNode owner, int idx, GetOutputValue method) : base(owner, idx, method) {

    }
}