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
        for (int i = 0; i < 5; i++) { //TODO insert longest Filestack length
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
        ConnectNode(from, fromSlot, to, toSlot);
        
        FNode fnTo = GetNode<FNode>(to);
        FNode fnFrom = GetNode<FNode>(from);
        try {

        //Disconnect existing Connection to Slot. This shoudln't be so hard Godot Wtf?!! 
        DisconnectNode(
            fnTo.inputs.ElementAt(toSlot).Value.connectedTo.owner.Name,  
            fnTo.inputs.ElementAt(toSlot).Value.connectedTo.idx,
            to, 
            toSlot);
        } catch {
            // Not Connected
        }
        
        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = fnFrom.outputs.ElementAt(fromSlot).Value;
        fnTo.GetChild(toSlot + fnTo.outputs.Count).GetChild<Control>(1).Visible = false;
    }

    public void OnDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
        FNode fnTo = GetNode<FNode>(to);
        fnTo.inputs.ElementAt(toSlot).Value.connectedTo = null;
        fnTo.GetChild(toSlot + fnTo.outputs.Count).GetChild<Control>(1).Visible = true;
    }

    public void OnAddNode(FNode fn) {
        FNode fd = (FNode)fn.Duplicate();
        fd.Offset = ScrollOffset + RectSize / 2f;
        AddChild(fd);
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
        
        for (int i=0; i < GetChildCount(); i++) {
            var fn = GetChild(i);
            if (fn is FNode) {
                if ((fn as FNode).Selected) {
                    DeleteNode(fn as FNode);
                }
            }
        }
    }
}
