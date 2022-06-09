using System.Collections.Generic;
using Godot;
using System.Linq;


public abstract class FNode : GraphNode {

    public Dictionary<string, FInput> inputs;
    public Dictionary<string, FOutput> outputs;
    public bool isExecutiveNode = false;

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

    // If a Node isExecutiveNode, this Method will always be called regardless of connected Nodes.
    public virtual void ExecutiveMethod(){}
}

