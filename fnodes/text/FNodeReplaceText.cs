using Godot;

public class FNodeReplaceText : FNode
{
    public FNodeReplaceText() {
        HintTooltip = "Replace Semgents of a Text with other Segments";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Filter", new FInputBool(this, initialValue: true)},
            {"Replace1", new FInputString(this)},
            {"With1", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() 
            {
                string str = inputs["Text"].Get() as string;

                if (!(bool)inputs["Filter"].Get()) {
                    return str;
                }

                for (int i = 0; i < ((inputs.Count-1) / 2); i++) {
                    string replaceStr = (string)inputs["Replace"+(i+1)].Get();
                    string withStr = (string)inputs["With"+(i+1)].Get();
                    str = str.Replace(replaceStr, withStr);
                }

                return str;
            })},
        };
    }

    public override void _Ready()
    {
        base._Ready();
        Button plusButton = new Button();
        plusButton.Name = "PlusButton";
        plusButton.Text = "+";
        plusButton.Connect("pressed", this, nameof(AddLine));
        AddChild(plusButton);
    }

    void AddLine() {
        FNode.IdxReset(inputs.Count);
        int count = (inputs.Count - 1) / 2 + 1;
        inputs.Add("Replace" + count, new FInputString(this));
        UIUtil.AddInputUI(this, "Replace"+count, inputs["Replace" + count]);
        
        inputs.Add("With" + count, new FInputString(this));
        UIUtil.AddInputUI(this, "With"+count, inputs["With" + count]);

        MoveChild(GetNode("PlusButton"), GetChildCount()-1);
        SetSlot(GetChildCount()-2, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-3, true, 0, Colors.Orange, false, 0, Colors.Orange, null, null);
        SetSlot(GetChildCount()-1, false, 0, Colors.Red, false, 0, Colors.Red, null, null);
    }
}
