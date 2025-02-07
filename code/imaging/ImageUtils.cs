using Godot;
using ImageMagick;

public static class ImageUtils
{

    public static Image MagickImageToGDImage(MagickImage mImg) {

        mImg.Depth = 8;
        byte[] b = mImg.ToByteArray(MagickFormat.Rgba);

        Image im = Image.CreateFromData(mImg.Width, mImg.Height, false, Image.Format.Rgba8, b);
        return im; //that was... easier than expected
    }

    public static float MapRange(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static MagickColor ToMagickColor(this Color col) {
        return new MagickColor((ushort)(col.R * 65535), (ushort)(col.G * 65535), (ushort)(col.B * 65535), (ushort)(col.A * 65535));
    }

}


