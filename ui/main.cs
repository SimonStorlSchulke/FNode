using Godot;
using System;
using System.Linq;

public class Main : VBoxContainer
{
    public static Main inst;
    public Project currentProject;
    TabContainer prTabs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        inst = this;
        prTabs = GetNode<TabContainer>("ProjectTabs");
        currentProject = prTabs.GetChild<Project>(prTabs.CurrentTab);
        GetTree().Connect("files_dropped", this, nameof(OnFilesDropped));
    }

    public void OnFilesDropped(string[] files, int screen) {
        int i = 0;
        if (GetGlobalMousePosition().x > RectGlobalPosition.x && GetGlobalMousePosition().y > RectGlobalPosition.y) {
            foreach (string f in files) {
                //Magic Numbers Bad
                FNodeFileInfo fi = new FNodeFileInfo();
                fi.Offset = (currentProject.NodeTree.GetLocalMousePosition() + currentProject.NodeTree.ScrollOffset) / currentProject.NodeTree.Zoom;
                fi.Offset += new Vector2(i * 340, 0);
                currentProject.NodeTree.AddChild(fi);
                fi.GetChild(7).GetChild<LineEdit>(1).Text = f;
                fi.Title = "FI " + new string(f.GetFile().Take(22).ToArray());;
                i++;
            }
        }
        else {
            //TODO add to File Stacks
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

    public void OnParseTree() {
        currentProject.GetNode<NodeTree>("NodeTree").EvaluateTree();
    }

}
