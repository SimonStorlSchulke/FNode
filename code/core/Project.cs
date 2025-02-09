using Godot;

public partial class Project : Control
{
    public NodeTree NodeTree;
    public TCFilestacks FileStacks;
    public SpinBox spIterations;
    public static int IdxEval = 0;
    public static int MaxNumFiles = 0;
    public static bool IsLastIteration = false;

    public override void _Ready() {
        NodeTree = GetNode<NodeTree>("NodeTree");   
        FileStacks = GetNode<TCFilestacks>("VBSideBar/TCFilestacks");   
        spIterations = GetNode<SpinBox>("VBSideBar/VBProjectSettings/HBIterations/SpIterations");   
    }
}
