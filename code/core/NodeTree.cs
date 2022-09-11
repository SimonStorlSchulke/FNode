using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class NodeTree : GraphEdit
{
    [Signal] public delegate void StartNextIteration();
    
    public override void _Ready() {
        //CallDeferred(nameof(HideControle));
    }

    public void HideControle() {
        (GetChild(1).GetChild(2) as Control).Visible = false;
    }

    public List<FNodeAwait> AwaiterNodes = new List<FNodeAwait>();

    public async void CheckAwaitersFinished() {

        // If there are no Awaiternodes in the tree, just start the evaluation...
        if (AwaiterNodes.Count == 0) {
            await EvaluateTree();
            return;
        }

        // ...If there are, this method is called for each awaiternode that has finished and checks if it was the last one
        foreach (var awaiterNode in AwaiterNodes) {
            if (awaiterNode.finished == false) {
                return;
            }
        }

        // ... If yes, tun the Nodetree (next iteration)
        await EvaluateTree();
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

    bool cancel = false;
    public async Task EvaluateTree() {
        cancel = false;

        Project.IdxEval = 0;
        
        GetTree().CallGroupFlags(
            (int)SceneTree.GroupCallFlags.Realtime,
            FNode.RunBeforeEvaluationGroup, 
            nameof(FNode.OnBeforeEvaluation));

        int iterations = (int)Math.Max(Main.Inst.CurrentProject.spIterations.Value, Project.MaxNumFiles);

        FProgressBar.inst.StartProgress();
        Main.preventRun = true;
        await Task.Run(() => {
        
        for (int i = 0; i < iterations; i++) {
            if (cancel) {
                FProgressBar.inst.EndProgress();
                Project.IdxEval = 0;
                Main.preventRun = false;
                return;
            }

            Project.IsLastIteration = Project.IdxEval > (iterations-2);

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
            Project.IdxEval++;
            //Task.Delay(1).Wait();
            FProgressBar.inst.ShowProgress((float)i / iterations);
        }
        });
        FProgressBar.inst.EndProgress();
        Main.preventRun = false;
        PuPreviewOps.ShowPreview();
    }

    public void OnConnectionRequest(string from, int fromSlot, string to, int toSlot) {

        if (from == to) {
            return;
        }
                
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

        Error err = ConnectNode(from, fromSlot, to, toSlot);

        if (err != 0) {
            return;
        }

        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = fnFrom.outputs.ElementAt(fromSlot).Value;
        Node inp = fnTo.GetChild(toSlot + fnTo.outputs.Count);
        inp.GetChild<Control>(1).Visible = false;
        if (inp.GetChildCount() > 2) {
            // Text and Date inputs for example have three children - all except the label need to be hidden;
            inp.GetChild<Control>(2).Visible = false;
        }
        
    }

    public void OnDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
        FNode fnTo = GetNode<FNode>(to);
        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = null;
        Node inp = fnTo.GetChild(toSlot + fnTo.outputs.Count);
        inp.GetChild<Control>(1).Visible = true;
        if (inp.GetChildCount() > 2) {
            inp.GetChild<Control>(2).Visible = true;
        }
    }

    public void OnAddNode(FNode fn, Vector2? offset = null) {

        fn.Offset = (offset == null) ? (ScrollOffset + new Vector2(100, 100)) / Zoom : (Vector2)offset;
        AddChild(fn);
        SetSelected(fn);
    }

    ///<summary>Creates a node by it's given typename. Mainly used for deserialization </summary> 
    public FNode OnAddNode(string nodetype, Vector2? offset = null, string name="") {
        var t = System.Type.GetType(nodetype);
        try {
            FNode fn = (FNode)Activator.CreateInstance(t);
        
            fn.Offset = (offset == null) ? Vector2.Zero : (Vector2)offset;
            AddChild(fn);
            if (name != "") {
                fn.Name = name;
            }
            return fn;
        } catch (System.Exception e) {
            Errorlog.Log("cannot create instance of unknown Type "+ nodetype + "\n\n" + e);
        }
        return null;
    }

    public void DeleteNode(FNode fn) {
        
        int idxOutputs = 0;
        foreach (KeyValuePair<string, FOutput> outp in fn.outputs) {
            int idxConnectedInputs = 0;
            foreach (var inp in outp.Value.GetConnectedInputs()) {
                DisconnectNode(fn.Name, idxOutputs, inp.owner.Name, inp.idx);
                inp.connectedTo = null;

                // unhide input-sockets that were previously connected to the now deleted node.
                var hb = inp.owner.GetChild(inp.idx + inp.owner.outputs.Count);
                hb.GetChild<Control>(1).Visible = true;
                if (hb.GetChildCount() > 2) {
                    hb.GetChild<Control>(2).Visible = true;
                }
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

    public Vector2 GetMouseOffset() {
        return (GetLocalMousePosition() + ScrollOffset) / Zoom;
    }

    public bool MouseOver() {
        Vector2 mPos = GetLocalMousePosition();
        return mPos.x > 0 && mPos.x < RectSize.x && mPos.y > 0 && mPos.y < RectSize.y;
    }

    public override void _Input(InputEvent e) {
        // Cancel tree evaluation 
        if (e.IsActionPressed("ui_cancel")) {
            cancel = true;
        }
    }

}
