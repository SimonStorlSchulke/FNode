using Godot;

public partial class FNodeFileExists : FNode
{
    public FNodeFileExists() {
        category = "File";
        TooltipText = "Returns true if a file with 'Filename' exists in the given 'Folder'";
        
        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Folder", new FInputString(this)},
            {"Filename", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "File", new FOutputBool(this, delegate() {
                string path = FileUtil.JoinPaths(inputs["Folder"].Get<string>(), inputs["Filename"].Get<string>());
                return System.IO.File.Exists(path);
            })
            }
        };
    }
}