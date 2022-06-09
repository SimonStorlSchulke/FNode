using Godot;
using System.Collections.Generic;
using System.Linq;

public class NodeTree : GraphEdit
{

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GetTree().Connect("files_dropped", this, nameof(OnFilesDropped));
    }

    public void OnFilesDropped(string[] files, int screen) {
        int i = 0;
        if (GetGlobalMousePosition().x > RectGlobalPosition.x && GetGlobalMousePosition().y > RectGlobalPosition.y) {
            foreach (string f in files) {
                //Magic Numbers Bad
                FNodeFileInfo fi = new FNodeFileInfo();
                fi.Offset = (GetLocalMousePosition() + ScrollOffset) / Zoom;
                fi.Offset += new Vector2(i * 340, 0);
                AddChild(fi);
                fi.GetChild(7).GetChild<LineEdit>(1).Text = f;
                fi.Title = "FI " + new string(f.GetFile().Take(22).ToArray());;
                i++;
            }
        }
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

    public void TestTree(string root) {
        FNode n = GetNode<FNode>(root);
        EvaluateTree(n);
    }

    public void EvaluateTree(FNode Root) {

        foreach (Node fn in GetChildren()) {
            if (fn is FNode) {
                if ((fn as FNode).isExecutiveNode) {
                    (fn as FNode).ExecutiveMethod();
                }
            }
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
        AddChild(fn.Duplicate());
    }

    public void DeleteNode(FNode fn) {
        
        int idxOutputs = 0;
        foreach (KeyValuePair<string, FOutput> outp in fn.outputs) {
            int idxConnectedInputs = 0;
            foreach (var inp in outp.Value.ConnectedTo()) {
                DisconnectNode(fn.Name, idxOutputs, inp.owner.Name, inp.idx);
                inp.connectedTo = null;
                inp.owner.GetChild(inp.idx).GetChild<Control>(1).Visible = true;
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
