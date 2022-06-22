using Godot;
using System;
using ImageMagick;

public static class ImageUtils {
    
    public static Image MagickImageToGDImage(MagickImage mImg, int width=0) {

        string viewerDir = FileUtil.JoinPaths(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "FNode/temp");
        FileUtil.CreateDirIfNotExisting(viewerDir);

        string imgPath = FileUtil.JoinPaths(viewerDir, "fnViewerTempImg-"+Guid.NewGuid() + ".webp");

        if (width != 0 && width < mImg.Width) {
            mImg.Scale(width, width);
        }
        mImg.Write(imgPath);

        Godot.Image testImage = new Godot.Image();
        testImage.Load(imgPath);
        try {
            System.IO.File.Delete(imgPath);
        } catch(System.Exception e) {
            Errorlog.Log("Failed deleting temporary Image-Viewer File", e);
        }
        return testImage;
    }
}
