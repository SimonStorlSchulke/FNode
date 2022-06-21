using Godot;
using System;

public class FNodeListComparisons : FNode
{
    string longestString = "";
    int longestStringIdx;
    string shortesstString = "";
    int shortesstStringIdx;
    float biggestNumber = -Mathf.Inf;
    int biggestNumberIdx;
    float smallestNumer = Mathf.Inf;
    int smallestNumerIdx;
    public FNodeListComparisons()
    {
        HintTooltip = "Retrieve an Item from a List. The output is of a variable Type.";
        category = "List";

        FNode.IdxReset();
        inputs = new System.Collections.Generic.Dictionary<string, FInput>() {
            {"List", new FInputList(this)},
        };

        FNode.IdxReset();
        outputs = new System.Collections.Generic.Dictionary<string, FOutput>() {
            {
            "Biggest Number", new FOutputFloat(this, delegate() {
                return biggestNumber;
            })},
            {
            "Biggest Number Idx", new FOutputInt(this, delegate() {
                return biggestNumberIdx;
            })},
            {
            "Smallest Number", new FOutputFloat(this, delegate() {
                return smallestNumer;
            })},
            {
            "Smallest Number Idx", new FOutputInt(this, delegate() {
                return smallestNumerIdx;
            })},
            {
            "Longest Text", new FOutputString(this, delegate() {
                return longestString;
            })},
            {
            "Longest Text Idx", new FOutputInt(this, delegate() {
                return longestStringIdx;
            })},
            {
            "Shortest Text", new FOutputString(this, delegate() {
                return shortesstString;
            })},
            {
            "Shortest Text idx", new FOutputInt(this, delegate() {
                return shortesstStringIdx;
            })},
        };
    }

    public override void OnNextIteration() {

        int i = 0;
        foreach (var item in (Godot.Collections.Array)inputs["List"].Get()) {
            try {
                if (((string)item).Length > longestString.Length) {
                    longestString = (string)item;
                    longestStringIdx = i;
                }
            } catch {}
            i++;
        }

        i = 0;
        foreach (var item in (Godot.Collections.Array)inputs["List"].Get()) {
            try {
                if (((string)item).Length < shortesstString.Length || shortesstString == "") {
                    shortesstString = (string)item;
                    shortesstStringIdx = i;
                }
            }
            catch {}
            i++;
        }
        var smallestNumTup = MathUtil.VarMin((Godot.Collections.Array)inputs["List"].Get());
        var biggestNumTup = MathUtil.VarMax((Godot.Collections.Array)inputs["List"].Get());

        smallestNumer = smallestNumTup.Item1;
        smallestNumerIdx = smallestNumTup.Item2;
        biggestNumber = biggestNumTup.Item1;
        biggestNumberIdx = biggestNumTup.Item2;
    }

    public override void OnBeforeEvaluation() {
        longestString = "";
        shortesstString = "";
        smallestNumer = Mathf.Inf;
        biggestNumber = -Mathf.Inf;

    }
}