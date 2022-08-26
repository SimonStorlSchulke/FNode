using Godot;

///<summary>Popup to list all File operations that would be done if the Nodetree was run in non-preview mode</summary>
public class PuPreviewOps : WindowDialog
{
    [Export] NodePath NPFilesDeleted;
    static RichTextLabel filesDeleted;

    [Export] NodePath NPFilesMoved;
    static RichTextLabel filesMoved;

    [Export] NodePath NPFilesCreated;
    static RichTextLabel filesCreated;

    public static PuPreviewOps inst;

    public override void _Ready() {
        filesDeleted = GetNode<RichTextLabel>(NPFilesDeleted);
        filesMoved = GetNode<RichTextLabel>(NPFilesMoved);
        filesCreated = GetNode<RichTextLabel>(NPFilesCreated);
        inst = this;
        AddToGroup(FNode.RunBeforeEvaluationGroup);
    }

    public static void ShowPreview() {
        bool fDel = filesDeleted.Text != "";
        bool fMov = filesMoved.Text != "";
        bool fCre = filesCreated.Text != "";

        if (fDel || fMov || fCre) {
            inst.PopupCentered();
        }

        filesDeleted.GetParent<Control>().Visible = fDel;
        filesMoved.GetParent<Control>().Visible = fMov;
        filesCreated.GetParent<Control>().Visible = fCre;
    }

    public static void AddFileDeleted(string path) {
        filesDeleted.Text += $"'{path.GetFile()}' in '{path.GetBaseDir()}'";
        if (!System.IO.File.Exists(path)) {
            filesDeleted.Text += " (which.. doesn't exist yet)";
        }
        filesDeleted.Text += "\n";
    }

    public static void AddFileMoved(string pathFrom, string pathTo) {
        filesMoved.Text += $"'{pathFrom.GetFile()}' to '{pathTo}'";
        if (System.IO.File.Exists(pathTo)) {
            filesMoved.Text += " (overwriting existing file)";
        }
        filesMoved.Text += "\n";
    }

    public static void AddFileCreated(string path) {
        filesCreated.Text += $"Create file '{path.GetFile()}' in '{path.GetBaseDir()}'";
        if (System.IO.File.Exists(path)) {
            filesCreated.Text += " (overwriting existing file)";
        }
        filesCreated.Text += "\n";
    }


    public static void OnBeforeEvaluation() {
        filesDeleted.Text = filesMoved.Text = filesCreated.Text = "";
    }
}
