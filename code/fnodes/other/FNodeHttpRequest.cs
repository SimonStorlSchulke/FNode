using Godot;

public partial class FNodeHttpRequest : FNodeAwait
{
    object jsonResult = "";

    public FNodeHttpRequest() {
        category = "Other";
        TooltipText = "Retrieve Data from the given URL.\nThis Node is evaluated only once before starting NodeTree evaluation and outputs the same data for each iteration";

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
        jsonResult = System.Text.Encoding.UTF8.GetString(body);
        Finished();
    }

    public override void _Ready()
    {
        base._Ready();
        HTTPHandler.inst.Connect("request_completed", new Callable(this, nameof(OnRequestCompleted)));
    }

    public override void WaitFor() {
        base.WaitFor();
        OnGetRestAPI();
    }
}
