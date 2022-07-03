using Godot;
using System;

/// <summary> parent class of all Nodes that implement an operation that needs to be waited for at the start of NodeTree evaluation. </summary>
public abstract class FNodeAwait : FNode {

    public bool finished;
    public override void _Ready() {
        base._Ready();
        Main.Inst.CurrentProject.NodeTree.AwaiterNodes.Add(this);
        AddToGroup("awaiter_nodes_group");
    }
    public override void _ExitTree() {
        Main.Inst.CurrentProject.NodeTree.AwaiterNodes.Remove(this);
    }

    public void Finished() {
        finished = true;
        Main.Inst.CurrentProject.NodeTree.CheckAwaitersFinished();
    }

    public override void OnBeforeEvaluation() {
        finished = false;
    }

    public virtual void WaitFor() {}
}
