using Godot;

public class FNodeReplaceText : FNode, IFNodeVarInputSize
{
    public FNodeReplaceText() {
        HintTooltip = "Replace Semgents of a Text with other Segments";
        category = "Text";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Toggle", new FInputBool(this, initialValue: true)},
            {"Replace1", new FInputString(this)},
            {"With1", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() 
            {
                string str = inputs["Text"].Get<string>();

                if (!(bool)inputs["Toggle"].Get<bool>()) {
                    return str;
                }

                for (int i = 0; i < ((inputs.Count-1) / 2); i++) {
                    string replaceStr = inputs["Replace"+(i+1)].Get<string>();
                    string withStr = inputs["With"+(i+1)].Get<string>();
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
        plusButton.Connect("pressed", this, nameof(AddInput));
        AddChild(plusButton);
    }

    void AddInput() {
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

    public void SetInputSize(int size) {

        int pairsToAdd = (size - inputs.Count) / 2;

        if (size > inputs.Count) {
            for (int i = 0; i < pairsToAdd; i++) {
                AddInput();
            }
        }
    }
}
