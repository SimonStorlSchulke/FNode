using System.Net.Http.Headers;
using Godot;
using System;

public class HTTPHandler : HTTPRequest
{
    public static HTTPHandler inst;
    public override void _Ready() {
        inst = this;
    }

}
