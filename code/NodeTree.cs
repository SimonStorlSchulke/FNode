using System;
using Godot;
using System.Collections.Generic;
using System.Linq;


public class NodeTree : GraphEdit
{
    [Signal] public delegate void StartNextIteration();

    public override void _Ready() {
        //CallDeferred(nameof(HideControle));
    }


    public void HideControle() {
        (GetChild(1).GetChild(2) as Control).Visible = false;
    }

    public List<FNode> GetFNodes() {

        List<FNode> fNodes = new List<FNode>();

        foreach (var node in GetChildren()) {
            if (node is FNode) {
                fNodes.Add(node as FNode);
            }
        }

        return fNodes;
    }

    public Vector2 GetMouseOffset() {
        return (GetLocalMousePosition() + ScrollOffset) / Zoom;
    }

    public bool MouseOver() {
        Vector2 mPos = GetLocalMousePosition();
        return mPos.x > 0 && mPos.x < RectSize.x && mPos.y > 0 && mPos.y < RectSize.y;
    }

    public void EvaluateTree() {
        Project.idxEval = 0;
        Project.maxNumFiles = 1;

        foreach (var fileList in Main.inst.currentProject.FileStacks.Stacks) {
            if (fileList.Count > Project.maxNumFiles)
                Project.maxNumFiles = fileList.Count;
        }
        
        GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.RunBeforeEvaluationGroup, nameof(FNode.OnBeforeEvaluation));


        //(GetChild(1).GetChild(2) as Control).Visible = false; // Hide unnecessary Nodeeditor Controlls

        

        int iterations = (int)Math.Max(Project.spIterations.Value, Project.maxNumFiles);
        
        for (int i = 0; i < iterations; i++) {

            EmitSignal(nameof(StartNextIteration));
            GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.RunBeforeIterationGroup, nameof(FNode.OnNextIteration));

            foreach (Node fn in GetChildren()) {
                if (fn is FNode)
                    (fn as FNode).ExecutiveMethodRan = false;
            }

            foreach (Node fn in GetChildren()) {
                if (fn is FNode) {
                    //Only run Executive Methods if they haven't been called before by the Tree Evaluation (this might be necessary for Nodes that return created Files etc.)
                    if ((fn as FNode).HasExecutiveMethod() && !(fn as FNode).ExecutiveMethodRan) { 
                        (fn as FNode).ExecutiveMethod();
                    }
                }
            }
            Project.idxEval++;
        }
    }

    public void OnConnectionRequest(string from, int fromSlot, string to, int toSlot) {
                
        FNode fnTo = GetNode<FNode>(to);
        FNode fnFrom = GetNode<FNode>(from);

            try {
                //Disconnect existing Connection to Slot. This shoudln't be so hard Godot Wtf?!! 
                DisconnectNode(
                    fnTo.inputs.ElementAt(toSlot).Value.connectedTo.owner.Name,  
                    fnTo.inputs.ElementAt(toSlot).Value.connectedTo.idx,
                    to, 
                    toSlot);
            } catch {/*Nothing Connected - just trying and catching is probably cheaper than checking if a connection exists first.*/}

        ConnectNode(from, fromSlot, to, toSlot);

        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = fnFrom.outputs.ElementAt(fromSlot).Value;
        fnTo.GetChild(toSlot + fnTo.outputs.Count).GetChild<Control>(1).Visible = false;
    }

    public void OnDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
        FNode fnTo = GetNode<FNode>(to);
        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = null;
        fnTo.GetChild(toSlot + fnTo.outputs.Count).GetChild<Control>(1).Visible = true;
    }

    public void OnAddNode(FNode fn, Vector2? offset = null) {

        fn.Offset = (offset == null) ? (ScrollOffset + new Vector2(100, 100)) / Zoom : (Vector2)offset;
        AddChild(fn);
        SetSelected(fn);
    }

    // Mainly used for Loading
    public FNode OnAddNode(string nodetype, Vector2? offset = null) {
        var t = System.Type.GetType(nodetype);
        FNode fn = (FNode)Activator.CreateInstance(t);
        fn.Offset = (offset == null) ? Vector2.Zero : (Vector2)offset;
        AddChild(fn);
        return fn;
    }

    public void DeleteNode(FNode fn) {
        
        int idxOutputs = 0;
        foreach (KeyValuePair<string, FOutput> outp in fn.outputs) {
            int idxConnectedInputs = 0;
            foreach (var inp in outp.Value.ConnectedTo()) {
                DisconnectNode(fn.Name, idxOutputs, inp.owner.Name, inp.idx);
                inp.connectedTo = null;
                inp.owner.GetChild(inp.idx + inp.owner.outputs.Count).GetChild<Control>(1).Visible = true;
                idxConnectedInputs++;
            }
            idxOutputs++;
        }
        
        foreach (KeyValuePair<string, FInput> inp in fn.inputs) {

            if (inp.Value.connectedTo != null) {
                DisconnectNode(
                    inp.Value.connectedTo.owner.Name,
                    inp.Value.connectedTo.idx,
                    inp.Value.owner.Name,
                    inp.Value.idx);
            }
        }
        fn.QueueFree();
    }

    public void OnDeleteRequest() {

        foreach (var n in GetSelectedNodes()) {
            DeleteNode(n);
        }
    }

    public List<FNode> GetSelectedNodes() {
        List<FNode> selected = new List<FNode>();
        for (int i=0; i < GetChildCount(); i++) {
            var fn = GetChild(i);
            if (fn is FNode) {
                if ((fn as FNode).Selected) {
                    selected.Add(fn as FNode);
                }
            }
        }
        return selected;
    }

    public FNode GetFirstSelectedNode() {
        for (int i=0; i < GetChildCount(); i++) {
            var fn = GetChild(i);
            if (fn is FNode) {
                if ((fn as FNode).Selected) {
                    return fn as FNode;
                }
            }
        }
        return null;
    }
}
