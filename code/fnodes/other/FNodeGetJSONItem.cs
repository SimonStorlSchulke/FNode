using Godot;
using Godot.Collections;
using System;

public class FNodeGetJSONItem: FNode
{

    static object stringify(object jsonItem) {
        var jsonType = jsonItem.GetType();

        if (jsonType == typeof(System.String)) {
            return jsonItem as string;
        }

        if (jsonType == typeof(Godot.Collections.Array)) {
            return ((Godot.Collections.Array)jsonItem);
        }

        return JSON.Print(jsonItem as Godot.Collections.Dictionary);
    }

    public FNodeGetJSONItem() {
        HintTooltip = "";
        category = "Other";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"JSON", new FInput(this)},
            {"Key", new FInputString(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutput(this, delegate() {
                try {
                    string key = inputs["Key"].Get<string>();
                    var jsonResult = inputs["JSON"].Get<object>();
                    var jsonType = jsonResult.GetType();

                    if (jsonType == typeof(System.String)) {
                        jsonResult = JSON.Parse(jsonResult as string).Result;
                        jsonType = jsonResult.GetType();
                    }


                    if (jsonType == typeof(Godot.Collections.Dictionary)) {
                        if ((jsonResult as Dictionary).Contains(key)) {
                            return stringify((jsonResult as Dictionary)[key]);
                        }
                    }

                    else if (jsonType == typeof(Godot.Collections.Array)) {
                        return stringify((jsonResult as Godot.Collections.Array)[key.ToInt()]);
                    }
                    return "Unknown JSON Type";

                } catch (System.Exception e) {
                    Errorlog.Log(e);
                    return "";
                }
            })},
        };
    }
}
