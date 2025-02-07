using Godot;

public partial class OnlineHelpLinks : RichTextLabel
{
    public void OnMetaClicked(object meta) {
        try {
            //Open Link in Browser
            System.Diagnostics.Process.Start((string)meta);
        } catch (System.Exception e) {
            Errorlog.Log(e);
        }
    }
}
