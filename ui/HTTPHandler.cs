using System.Net.Http.Headers;
using Godot;
using System;

public partial class HTTPHandler : HttpRequest
{
    public static HTTPHandler inst;
    public override void _Ready() {
        inst = this;
    }

}
