using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeGetImage : FNode
{
    public FNodeGetImage() {
        HintTooltip = "Multiple Math Operations";
        category = "Math";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Output Path", new FInputString(this, description: "if this is left empty, the file will be saved at the original files path")},
            {"Remove original", new FInputBool(this, initialValue: true)},
            {"Quality", new FInputInt(this, initialValue: 75, min: 16, max: 100, description: "The less Qualitx, the larger the file Size. Not used by all filetypes.")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Result", new FOutputFloat(this, delegate() {

                return 0f;
            })},
        };
    }

    public override void ExecutiveMethod() {
        using (MagickImage image = new MagickImage((string)inputs["Image"].Get())) {
            string origPath = (string)inputs["Image"].Get();
            string nPath = (string)inputs["Output Path"].Get();
            string path = nPath == "" ? origPath.GetBaseDir() + "\\" + System.IO.Path.GetFileNameWithoutExtension(origPath) : nPath;
            image.Quality = (int)inputs["Quality"].Get();
            image.Write(path + "." + GetSelectedOption("To Type"));
        }
        base.ExecutiveMethod();
    }

    
    public override void _Ready()
    {
        base._Ready();
        AddOptionEnum(
            "To Type",

            new string[] {
                "WebP",
                "jpg",
                "tiff",
                "png",
                "exr",
            });
    }
}
