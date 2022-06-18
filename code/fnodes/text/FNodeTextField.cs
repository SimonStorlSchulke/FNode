using Godot;

public class FNodeTextField : FNode
{
    TextEdit TEEdit;
    public FNodeTextField() {
        category = "Text";        
        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Includes", new FOutputString(this, delegate() {
                return TEEdit.Text;
            })},
        };
    }

    public override void _Ready()
    {
        base._Ready();
        TEEdit = new TextEdit();
        TEEdit.Name = "TextEdit";
        TEEdit.RectMinSize = new Vector2(400, 400);
        AddChild(TEEdit);
    }
}
