using Godot;
using ImageMagick;

public static class ImageUtils
{

    public static Image MagickImageToGDImage(MagickImage mImg) {

        mImg.Depth = 8;
        byte[] b = mImg.ToByteArray(MagickFormat.Rgba);

        Godot.Image im = new Godot.Image();
        im.CreateFromData(mImg.Width, mImg.Height, false, Image.Format.Rgba8, b);
        return im; //that was... easier than expected
    }

    public static float MapRange(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}


