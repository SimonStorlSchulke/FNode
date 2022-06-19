using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Main : VBoxContainer
{
    [Signal]
    public delegate void StartParsing();

    [Signal]
    public delegate void MainReady();

    public static Main inst;
    public Project currentProject;
    PackedScene projectScene;
    [Export] NodePath NPProjectTabs;
    public TabContainer projectTabs;

    [Export] NodePath NPAddNodeButtons;
    TCAddNodesPanel addNodeButtons;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        inst = this;
        GetTree().Connect("files_dropped", this, nameof(OnFilesDropped));
        projectScene = GD.Load<PackedScene>("res://ui/Project.tscn");
        projectTabs = GetNode<TabContainer>(NPProjectTabs);
        currentProject = projectTabs.GetChild<Project>(projectTabs.CurrentTab);
        projectTabs.Connect("tab_selected", this, nameof(OnChangeProject));
        addNodeButtons = GetNode<TCAddNodesPanel>(NPAddNodeButtons);
        addNodeButtons.CreateButtons();
    }

    
    public void OnAddNodeFromUI(FNode fn, bool autoconnect = true) {
        
        var fnSel = currentProject.NodeTree.GetFirstSelectedNode();
        if (fnSel == null) {
            if (currentProject.NodeTree.MouseOver()) currentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode, currentProject.NodeTree.GetMouseOffset());
            else currentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode);
            return;
        }

        if (fnSel.outputs.Count > 0 && fn.inputs.Count > 0 && autoconnect) {
            FNode fnDup = (FNode)fn.Duplicate();
            fnDup.Offset = fnSel.Offset + new Vector2(fnSel.RectSize.x + 50, 0);
            
            currentProject.NodeTree.AddChild(fnDup);

            // Search matching Slottypes and connect them if aviable
            foreach (var outp in fnSel.outputs) {
                foreach (var inp in fnDup.inputs) {
                    if (inp.Value.slotType == outp.Value.slotType) {
                        currentProject.NodeTree.SetSelected(fnDup);
                        currentProject.NodeTree.OnConnectionRequest(fnSel.Name, outp.Value.idx, fnDup.Name, inp.Value.idx);
                        return;
                    }
                }
            }
            
            currentProject.NodeTree.SetSelected(fnDup);
            currentProject.NodeTree.OnConnectionRequest(fnSel.Name, 0, fnDup.Name, 0);
            return;
        }
        
        else {
            if (currentProject.NodeTree.MouseOver()) currentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode, currentProject.NodeTree.GetMouseOffset());
            else currentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode);
        }
    }

    void OnChangeProject(int idx) {
        this.currentProject = projectTabs.GetChild<Project>(idx);
    }

    public static Project NewProject(string name) {
        Project pr = inst.projectScene.Instance<Project>();
        pr.Name = name;
        inst.projectTabs.AddChild(pr);
        inst.projectTabs.SetTabTitle(pr.GetIndex(), name);
        inst.projectTabs.CurrentTab = inst.projectTabs.GetChildCount()-1;
        Main.inst.currentProject = pr;
        return pr;
    }

    public void OnFilesDropped(string[] files, int screen) {
        int i = 0;
        if (currentProject.NodeTree.MouseOver()) {
            foreach (string f in files) {
                //Magic Numbers Bad
                FNodeFileInfo fi = new FNodeFileInfo();
                fi.Offset = currentProject.NodeTree.GetMouseOffset();
                fi.Offset += new Vector2(i * 340, 0);
                currentProject.NodeTree.AddChild(fi);
                fi.GetChild(10).GetChild<LineEdit>(1).Text = f;
                fi.Title = "FI " + new string(f.GetFile().Take(22).ToArray());;
                i++;
            }
        }
        else {
            List<Tuple<bool, string>> l = new List<Tuple<bool, string>>();
            int selectedStack = currentProject.FileStacks.CurrentTab;
            /*
            foreach (string f in files) {
                currentProject.FileStacks.AddFile(f, selectedStack);
            }*/
            currentProject.FileStacks.GetChild<FileList>(selectedStack).AddFiles(files);
            //currentProject.FileStacks.OnUpdateUI(selectedStack);
        }
    }

    public void OnSizeChanged() {
        float scaleMul = 0.5f;
        Vector2 winSize = OS.WindowSize;
        float scale = Mathf.Min(winSize.x / 1920f, winSize.y / 1080f);
        scale /= scaleMul;
        GD.PrintT(winSize);
        RectScale = new Vector2(1f * (1f / scale), 1f * (1f / scale));
        GD.PrintT(RectScale);
        RectSize = winSize * RectScale * (16f / 9f);// / RectScale;//(2f * RectScale);// * RectScale;
    }

    public void OnParseTree(bool preview) {
        EmitSignal(nameof(StartParsing));
        currentProject.GetNode<NodeTree>("NodeTree").EvaluateTree(preview);
    }
}
