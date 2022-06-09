using System.Collections.Generic;
using Godot;
using System.Linq;


public delegate object GetOutputValue();

public class FOutput {
    public FNode owner;
    public GetOutputValue Get;
    public int idx;
    public FOutput(FNode owner, GetOutputValue method) {
        this.owner = owner;
        this.idx = FNode.IdxNext();
        Get = method;
    }

    
    public List<FInput> ConnectedTo() {
        NodeTree nt = owner.GetParent<NodeTree>();
        Godot.Collections.Array links = nt.GetConnectionList();
        List<FInput> inpList = new List<FInput>();
        
        // There must be a better way...
        foreach (Godot.Collections.Dictionary link in links) {
            if ((string)link["from"] == owner.Name && (int)link["from_port"] == idx) {
                inpList.Add(nt.GetNode<FNode>((string)link["to"]).inputs.ElementAt((int)link["to_port"]).Value);
            }
        }
        return inpList;
    }

}

public class FOutputInt : FOutput {
    public FOutputInt(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}

public class FOutputFloat : FOutput {
    public FOutputFloat(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}

public class FOutputBool : FOutput {
    public FOutputBool(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}

public class FOutputString : FOutput {
    public FOutputString(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}

public class FOutputFile : FOutput {
    public FOutputFile(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}

public class FOutputDate : FOutput {
    public FOutputDate(FNode owner, GetOutputValue method) : base(owner, method) {

    }
}