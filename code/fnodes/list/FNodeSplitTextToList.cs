using Godot;
using System.Collections.Generic;

public partial class FNodeSplitTextToList : FNode
{
    public FNodeSplitTextToList() {
        TooltipText = "At each occurence of the given character, Split the Text and add it to the List";
        category = "List";

        IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Text", new FInputString(this)},
            {"Split at", new FInputString(this, initialValue: ",")},
        };

        IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "List", new FOutputList(this, delegate() {
                string[] arr = inputs["Text"].Get<string>().Split(inputs["Split at"].Get<string>());
                List<object> gdArr = new (arr);
                return gdArr;
            })},
        };
    }
}
