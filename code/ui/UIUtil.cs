using Godot;
using System;

public static class UIUtil
{
    public static string SnakeCaseToWords(string str) {
        return System.Text.RegularExpressions.Regex.Replace(str, "([A-Z0-9]+)", " $1").Trim();
    }

    ///<summary>Turns "I am a looong string" into "I am a looong st.." based on the font and given width</summary>
    public static string GetOverflowDots(string str, Godot.Font font, float width) {
        string dotString = str;
        bool removed = false;
        while (width - font.GetStringSize(dotString).x < 0) {
            dotString = dotString.Substring(0, dotString.Length-1);
            removed = true;
        }
        return removed ? dotString + ".." : dotString;
    }

    ///<summary>Let tabs fill all aviable horizontal space. Possible to use as Extension Method</summary>
    public static void ExpandTabs(this TabContainer tc) {
        float tabNamesCombinedWidth = 0;

        for (int i = 0; i < tc.GetTabCount(); i++) {
            tabNamesCombinedWidth += tc.GetFont("font").GetStringSize(tc.GetTabTitle(i)).x; 
        }

        float tabMargin = (tc.RectSize.x - tabNamesCombinedWidth) / (tc.GetChildCount() * 2);

        tc.GetStylebox("tab_fg").ContentMarginRight = 
            tc.GetStylebox("tab_bg").ContentMarginRight = 
            tc.GetStylebox("tab_fg").ContentMarginLeft = 
            tc.GetStylebox("tab_bg").ContentMarginLeft = tabMargin;
    }
}

class DateLabel : Label {
    public void SetDate(Godot.Reference date) {
        Text = (string)date.Call("date", new object[]{"DD.MM.YYYY"});
    }
}

class HbInput : HBoxContainer {

}

class HbOutput : HBoxContainer {

}

class HbOption : HBoxContainer {
    public OptionButton optionButton;
    public Label label;
    public HbOption(string name, OptionButton optionButton) {
        Name = name;
        this.optionButton = optionButton;
        label = new Label();
        label.Text = name + ": ";
        AddChild(label);
        AddChild(optionButton);
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
    }
}