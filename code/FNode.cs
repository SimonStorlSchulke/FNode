using System.Collections.Generic;
using Godot;
using System.Linq;


public abstract class FNode : GraphNode {

    public Dictionary<string, FInput> inputs;
    public Dictionary<string, FOutput> outputs;
    public override void _Ready()
    {
        ShowClose = true;
        Title = this.GetType().Name.Replace("FNode", "");
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
    public static void IdxReset() {
        indexerNum = 0;
    }
}

