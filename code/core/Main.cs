using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Main : Control
{
    [Signal]
    public delegate void StartParsing();

    [Signal]
    public delegate void MainReady();

    public static Main Inst;
    public Project CurrentProject;
    PackedScene projectScene;
    [Export] NodePath NPProjectTabs;
    public TabContainer ProjectTabs;

    [Export] NodePath NPAddNodeButtons;
    TCAddNodesPanel addNodeButtons;

    public static bool preventRun = false;

    public override void _Ready() {
        GetTree().Connect("files_dropped", this, nameof(OnFilesDropped));
        projectScene = GD.Load<PackedScene>("res://ui/Project.tscn");
        ProjectTabs = GetNode<TabContainer>(NPProjectTabs);
        CurrentProject = ProjectTabs.GetChild<Project>(ProjectTabs.CurrentTab);
        ProjectTabs.Connect("tab_selected", this, nameof(OnChangeProject));
        addNodeButtons = GetNode<TCAddNodesPanel>(NPAddNodeButtons);
        addNodeButtons.CreateButtons();
    }

    public override void _EnterTree() {
        Inst = this;
        float UIScale = OS.GetScreenSize().x > 2000 ? 0.85f : 0.6f;
        OS.WindowSize = OS.GetScreenSize() * 0.7f; //Always open window at 0.7% screen size
        OS.WindowPosition = OS.GetScreenSize() / 2 - OS.WindowSize / 2;
        GetTree().SetScreenStretch(SceneTree.StretchMode.Disabled, SceneTree.StretchAspect.Ignore, new Vector2(128, 128), UIScale);
    }

    /// <summary>  </summary>
    public void OnAddNodeFromUI(FNode fn, bool autoconnect = true) {
        
        var fnSel = CurrentProject.NodeTree.GetFirstSelectedNode();
        if (fnSel == null) {
            if (CurrentProject.NodeTree.MouseOver()) CurrentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode, CurrentProject.NodeTree.GetMouseOffset());
            else CurrentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode);
            return;
        }

        // If a fnode is currently selected, try to connect matching slottypes to the new fNode
        if (fnSel.outputs.Count > 0 && fn.inputs.Count > 0 && autoconnect) {
            FNode fnDup = (FNode)fn.Duplicate();

            // place new fnode right to the selected one
            fnDup.Offset = fnSel.Offset + new Vector2(fnSel.RectSize.x + 50, 0);
            
            CurrentProject.NodeTree.AddChild(fnDup);

            // search matching Slottypes and connect them if aviable
            foreach (var outp in fnSel.outputs) {
                foreach (var inp in fnDup.inputs) {
                    if (inp.Value.slotType == outp.Value.slotType) {
                        CurrentProject.NodeTree.SetSelected(fnDup);
                        CurrentProject.NodeTree.OnConnectionRequest(fnSel.Name, outp.Value.idx, fnDup.Name, inp.Value.idx);
                        return;
                    }
                }
            }
            
            CurrentProject.NodeTree.SetSelected(fnDup);
            CurrentProject.NodeTree.OnConnectionRequest(fnSel.Name, 0, fnDup.Name, 0);
            return;
        }
        
        else {
            // if no fnode is selected, ad it at the dragged location...
            if (CurrentProject.NodeTree.MouseOver()) CurrentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode, CurrentProject.NodeTree.GetMouseOffset());

            // or at the default location if it's just clicked
            else CurrentProject.NodeTree.OnAddNode(fn.Duplicate() as FNode);
        }
    }

    void OnChangeProject(int idx) {
        this.CurrentProject = ProjectTabs.GetChild<Project>(idx);
    }

    public static Project NewProject(string name) {
        Project pr = Inst.projectScene.Instance<Project>();
        pr.Name = name;
        Inst.ProjectTabs.AddChild(pr);
        Inst.ProjectTabs.SetTabTitle(pr.GetIndex(), name);
        Inst.ProjectTabs.CurrentTab = Inst.ProjectTabs.GetChildCount()-1;
        return pr;
    }

    public void CloseProject() {
        int current = Inst.ProjectTabs.CurrentTab;
        Inst.ProjectTabs.CurrentTab = current-1;
        ProjectTabs.GetChild(current).QueueFree();
    }

    public void OnFilesDropped(string[] files, int screen) {
        int i = 0;
        if (CurrentProject.NodeTree.MouseOver()) {
            foreach (string f in files) {
                //Magic Numbers Bad
                FNodeFileInfo fi = new FNodeFileInfo();
                fi.Offset = CurrentProject.NodeTree.GetMouseOffset();
                fi.Offset += new Vector2(i * 340, 0);
                CurrentProject.NodeTree.AddChild(fi);
                fi.GetChild(10).GetChild<LineEdit>(1).Text = f;
                fi.Title = "FI " + new string(f.GetFile().Take(22).ToArray());;
                i++;
            }
        }
        else {
            List<Tuple<bool, string>> l = new List<Tuple<bool, string>>();
            int selectedStack = CurrentProject.FileStacks.CurrentTab;
            CurrentProject.FileStacks.GetChild<FileList>(selectedStack).AddFiles(files);
        }
    }

    public void OnSizeChanged() {
        float scaleMul = 0.5f;
        Vector2 winSize = OS.WindowSize;
        float scale = Mathf.Min(winSize.x / 1920f, winSize.y / 1080f);
        scale /= scaleMul;
        RectScale = new Vector2(1f * (1f / scale), 1f * (1f / scale));
        RectSize = winSize * RectScale * (16f / 9f);
    }
    
    public bool preview {get; private set;}
    public void OnParseTree(bool preview) {
        this.preview = preview;
        if (preventRun) return;

        EmitSignal(nameof(StartParsing));

        /* This starts all Nodes that need to be waited for before evaluation (like REST API calls). 
        Each AwaiterNode then checks, if it was the last one finished and starts the NodeTree Evaluation if so.*/
        GetTree().CallGroupFlags((int)SceneTree.GroupCallFlags.Realtime, FNode.AwaiterNodesGroup, nameof(FNodeAwait.WaitFor));

        CurrentProject.NodeTree.CheckAwaitersFinished();
    }
}
