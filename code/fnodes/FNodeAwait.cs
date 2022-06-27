using Godot;
using System;

public abstract class FNodeAwait : FNode {

    public bool finished;
    public override void _Ready() {
        base._Ready();
        Main.inst.currentProject.NodeTree.awaiterNodes.Add(this);
        AddToGroup("awaiter_nodes_group");
    }
    public override void _ExitTree() {
        Main.inst.currentProject.NodeTree.awaiterNodes.Remove(this);
    }

    public void Finished() {
        finished = true;
        Main.inst.currentProject.NodeTree.CheckAwaitersFinished();
    }

    public override void OnBeforeEvaluation() {
        finished = false;
    }

    public virtual void WaitFor() {}
}
