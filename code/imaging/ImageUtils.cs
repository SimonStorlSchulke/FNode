using Godot;
using System;
using ImageMagick;

public static class ImageUtils {
    
    public static Image MagickImageToGDImage(MagickImage mImg, int width=0) {


        if (width != 0) {
            mImg.Scale(width, width);
        }
        mImg.Write("C:/Users/simon/Pictures/bg/t/t.webp");

        Godot.Image testImage = new Godot.Image();
        testImage.Load("C:/Users/simon/Pictures/bg/t/t.webp");
        return testImage;
    }
}
