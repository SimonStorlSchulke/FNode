using System.IO;
using ImageMagick;

public partial class FNodeGetImage : FNode
{
    public FNodeGetImage() {
        TooltipText = "Get Image Data if the incoming file is an image";
        category = "Img";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"File", new FInputFile(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Image", new FOutputImage(this, delegate() {
                try {
                    return new MagickImage(inputs["File"].Get<FileInfo>());
                } catch {
                    return null;
                }
            })},
        };
    }
}
