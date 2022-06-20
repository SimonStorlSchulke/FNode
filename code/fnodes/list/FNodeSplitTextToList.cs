using Godot;

public class FNodeSplitTextToList : FNode
{
    public FNodeSplitTextToList() {
        HintTooltip = "At each occurence of the given character, Split the Text and add it to the List";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Split at", new FInputString(this, initialValue: ",")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                string[] arr = ((string)inputs["Text"].Get()).Split((string)inputs["Split at"].Get());
                Godot.Collections.Array gdArr = new Godot.Collections.Array(arr);
                return gdArr;
            })},
        };
    }
}
