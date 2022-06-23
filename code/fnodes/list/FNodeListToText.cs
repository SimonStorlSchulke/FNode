using Godot;
using System;

public class FNodeListToText : FNode
{
    public FNodeListToText() {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
            {"Separator", new FInputString(this, description: "This Text will be added between each accumulated Text", initialValue: "[LINEBREAK]")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutputString(this, delegate() {

                string text = "";
                Godot.Collections.Array arr;
                
                arr = inputs["List"].Get<Godot.Collections.Array>();
                
                string sep = inputs["Separator"].Get<string>();
                sep = sep.Replace("[LINEBREAK]", "\n"); //TODO sanitize this...

                int i = 0;
                if (arr == null) {
                    return "";
                }
                foreach (var item in arr) {
                    text += i < arr.Count-1 ? item.ToString() + sep : item.ToString();
                    i++;
                }
                return text;
            })},
        };
    }
}
