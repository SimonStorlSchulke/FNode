using System.Collections.Generic;
using Godot;
using System.Linq;


public abstract class FNode : GraphNode {

    public Dictionary<string, FInput> inputs;
    public Dictionary<string, FOutput> outputs;
    public bool isExecutiveNode = false;

    public override void _Ready()
    {
        Title = this.GetType().Name.Replace("FNode", "");
        UIUtil.CreateUI(this);
    }

    // If a Node isExecutiveNode, this Method will always be called regardles of connected Nodes.
    public virtual void ExecutiveMethod(){}
}

