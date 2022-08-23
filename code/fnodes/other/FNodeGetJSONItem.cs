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

                string[] keychain = inputs["Key"].Get<string>().Split("\\");                
                var cJsonResult = inputs["JSON"].Get<object>();
                var cJsonType = cJsonResult.GetType();

                foreach (string key in keychain) {
                    //try {
                        if (cJsonType == typeof(System.String)) {
                            cJsonResult = JSON.Parse(cJsonResult as string).Result;
                            if (!(cJsonResult as Dictionary).Contains(key)) {
                                return "Invalid Key";
                            }
                            cJsonResult = stringify((cJsonResult as Dictionary)[key]);
                            cJsonType = cJsonResult.GetType();
                        }

                        else if (cJsonType == typeof(Godot.Collections.Dictionary)) {
                            if ((cJsonResult as Dictionary).Contains(key)) {
                                cJsonResult = stringify((cJsonResult as Dictionary)[key]);
                                cJsonType = cJsonResult.GetType();
                            }
                        }

                        else if (cJsonType == typeof(Godot.Collections.Array)) {
                            cJsonResult = stringify((cJsonResult as Godot.Collections.Array)[key.ToInt()]);
                            cJsonType = cJsonResult.GetType();
                        }
                   // }

                    //catch (System.Exception e) {
                    //    Errorlog.Log(e);
                    //    return "Invalid JSON";
                    //}
                }
                return stringify(cJsonResult);
            })},
        };
    }
}
