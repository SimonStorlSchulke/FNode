using Godot;
using ImageMagick;

public class FNodeSaveImageAs : FNode
{
    public FNodeSaveImageAs() {
        HintTooltip = "Save an Image to a certain path";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"Image", new FInputImage(this)},
            {"Output Folder", new FInputString(this, description: "if this is left empty, the file will be saved at the original files path")},
            {"Output Name", new FInputString(this, description: "if this is left empty, the file will be saved with the original files name")},
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
        string nFolder = inputs["Output Folder"].Get<string>();
        string nName = inputs["Output Name"].Get<string>();
        string filename = nName == "" ? System.IO.Path.GetFileNameWithoutExtension(origPath) : nName;
        string path = nFolder == "" ? origPath.GetBaseDir() : nFolder;
        string finalPath = path + "\\" + filename + "." + GetSelectedOption("To Type");
        image.Quality = inputs["Quality"].Get<int>();

        if (Main.Inst.preview) {
            PuPreviewOps.AddFileCreated(finalPath);
            base.ExecutiveMethod();
            return;
        }
        

        try {
            FileUtil.CreateDirIfNotExisting(nFolder);
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
