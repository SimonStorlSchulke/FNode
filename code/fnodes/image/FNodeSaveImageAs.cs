using System.IO;
using Godot;
using System;
using ImageMagick;

public class FNodeSaveImageAs : FNode
{
    public FNodeSaveImageAs() {
        HintTooltip = "Multiple Math Operations";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Output Path", new FInputString(this, description: "if this is left empty, the file will be saved at the original files path")},
            //{"Remove original", new FInputBool(this, initialValue: true)},
            {"Quality", new FInputInt(this, initialValue: 75, min: 16, max: 100, description: "The less Quality, the larger the file Size. Not used by all filetypes.")},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
        };
    }

    public override void ExecutiveMethod() {

        MagickImage image = inputs["Image"].Get<MagickImage>();
        if (image == null) {
            return;
        }
        string origPath = image.FileName;
        string nPath = inputs["Output Path"].Get<string>();
        string path = nPath == "" ? origPath.GetBaseDir() : nPath;
        FileUtil.CreateDirIfNotExisting(nPath);
        image.Quality = inputs["Quality"].Get<int>();
        string finalPath = path + "\\" + System.IO.Path.GetFileNameWithoutExtension(origPath) + "." + GetSelectedOption("To Type");

        if (Main.inst.currentProject.NodeTree.previewMode) {
            PuPreviewOps.AddFileCreated(finalPath);
            base.ExecutiveMethod();
            return;
        }
        try {
            image.Write(finalPath);
        } catch (ImageMagick.MagickException e) { //TODO test if there is any advantage over using System.Exception here
            Errorlog.Log(this, "Could not write image - " + e.Message);
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
                "tga",
                "psd",
                "exr",
            });
    }
}
