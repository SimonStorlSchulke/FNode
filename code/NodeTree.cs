using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class NodeTree : GraphEdit
{    

    public List<FNode> GetFNodes() {

        List<FNode> fNodes = new List<FNode>();

        foreach (var node in GetChildren()) {
            if (node is FNode) {
                fNodes.Add(node as FNode);
            }
        }

        return fNodes;
    }

    public void EvaluateTree() {
        Project.idxEval = 0;
        Project.maxNumFiles = 1;

        foreach (var fileList in Main.inst.currentProject.FileStacks.Stacks) {
            if (fileList.Count > Project.maxNumFiles)
                Project.maxNumFiles = fileList.Count;
        }

        for (int i = 0; i < Project.maxNumFiles; i++) { //TODO insert longest Filestack length
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

        //object v = Root.outputs["Name"].Get();
        //GD.Print(v);
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
        GD.Print("Disc");
        DisconnectNode(from, fromSlot, to, toSlot);
        FNode fnTo = GetNode<FNode>(to);
        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = null;
        fnTo.GetChild(toSlot + fnTo.outputs.Count).GetChild<Control>(1).Visible = true;
    }

    public void OnAddNode(FNode fn, Vector2? offset = null) {
        fn.Offset = (offset == null) ? ScrollOffset + RectSize / 2f : (Vector2)offset;
        AddChild(fn);
        SetSelected(fn);
    }

    public void OnAddNodeFromUI(FNode fn) {
        
        var fnSel = GetFirstSelectedNode();
        if (fnSel == null) {
            OnAddNode(fn.Duplicate() as FNode);
            return;
        }

        if (fnSel.outputs.Count > 0 && fn.inputs.Count > 0) {
            FNode fnDup = (FNode)fn.Duplicate();
            fnDup.Offset = fnSel.Offset + new Vector2(fnSel.RectSize.x + 50, 0);
            
            AddChild(fnDup);

            // Search matching Slottypes and connect them if aviable
            foreach (var outp in fnSel.outputs) {
                foreach (var inp in fnDup.inputs) {
                    if (inp.Value.slotType == outp.Value.slotType) {
                        SetSelected(fnDup);
                        OnConnectionRequest(fnSel.Name, outp.Value.idx, fnDup.Name, inp.Value.idx);
                        return;
                    }
                }
            }
            
            SetSelected(fnDup);
            OnConnectionRequest(fnSel.Name, 0, fnDup.Name, 0);
            return;
        }
        
        else {
            OnAddNode(fn.Duplicate() as FNode);
        }
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
