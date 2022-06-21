using Godot;
using System;

public static class MathUtil
{
    public static Tuple<float, int> VarMin(Godot.Collections.Array arr) {
        float min = Mathf.Inf;
        int i = 0;
        int idx = 0;
        foreach (var item in arr) {
            try {
                float v = System.Convert.ToSingle(item);
                if (v < min) {
                    min = v;
                    idx = i;
                }
            } catch {}
            i++;
        }
        return new Tuple<float, int>(min, idx);
    }

    public static Tuple<float, int> VarMax(Godot.Collections.Array arr) {
        float max = -Mathf.Inf;
        int i = 0;
        int idx = 0;
        foreach (var item in arr) {
            try {
                float v = System.Convert.ToSingle(item);
                if (v > max) {
                    max = v;
                    idx = i;
                }
            } catch {}
            i++;
        }
        return new Tuple<float, int>(max, idx);
    }
}
