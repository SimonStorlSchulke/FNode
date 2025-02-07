using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FNodeGetJSONItem: FNode
{
    static object validate(object jsonItem) {
        switch (jsonItem)
        {
            case System.String str:
                return str;
            case Godot.Collections.Array arr:
                return arr;
            case Godot.Collections.Dictionary dict:
                return dict;
            default:
                return jsonItem; //TODO migration
                //return JSON.Print(jsonItem as Godot.Collections.Dictionary);
        }
    }

    public FNodeGetJSONItem() {
        TooltipText = "";
        category = "Other";        

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"JSON", new FInputString(this, description: "The JSON string to parse", initialValue: "{\"myjson\":{\"nestedprop\":[\"Hello\",\"World\"]}}")},
            {"Key", new FInputString(this, description: "the key to get from the JSON.\nThis can be chained with \"\\\"", initialValue: "myjson\\nestedprop\\1")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutput(this, delegate() {

                string[] keychain = inputs["Key"].Get<string>().Split("\\");                
                var cJsonResult = inputs["JSON"].Get<object>();

                foreach (string key in keychain) {
                    try {

                        switch (cJsonResult)
                        {
                            case System.String str:
                                //cJsonResult = JSON.Parse(str).Result; //TODO migration
                                if (!(cJsonResult as Dictionary<string, object>).ContainsKey(key)) {
                                    return "Invalid Key";
                                }
                                cJsonResult = validate((cJsonResult as Dictionary<string, object>)[key]);
                                break;
                                case Godot.Collections.Dictionary dict:
                                    if ((cJsonResult as Dictionary<string, object>).ContainsKey(key)) {
                                        cJsonResult = validate(dict[key]);
                                    }
                                break;
                                case Godot.Collections.Array arr:
                                    cJsonResult = validate(arr[key.ToInt()]);
                                    break;
                            default:
                                break;
                        }
                    }

                    catch (System.Exception e) {
                        Errorlog.Log(e);
                        return "Invalid JSON";
                    }
                }
                return validate(cJsonResult);
            })},
        };
    }
}
