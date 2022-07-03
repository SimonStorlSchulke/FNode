using Godot;

public class FNodeGetRestAPI : FNodeAwait
{
    object jsonResult = "";

    public FNodeGetRestAPI() {
        category = "Other";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"URL", new FInputString(this, initialValue: "http://www.mocky.io/v2/5185415ba171ea3a00704eed")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Text", new FOutput(this, delegate() {
                return jsonResult;
            })},
        };
    }


    public void OnGetRestAPI() {
        HTTPHandler.inst.Request(inputs["URL"].Get<string>());
    }

    public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
        jsonResult = json.Result;
        Finished();
    }

    public override void _Ready()
    {
        base._Ready();
        HTTPHandler.inst.Connect("request_completed", this, nameof(OnRequestCompleted));
    }

    public override void WaitFor() {
        base.WaitFor();
        OnGetRestAPI();
    }
}
