using Godot;
using System;

namespace Tests.FNode.Math {


// Developers may use the [Title] attribute on a Test which will then use..
	// ..string as the name of the Test in the Results View
	
	// See target methods for more information


public class TestFNodeMath : WAT.Test
{
    public void InitializeTests() {

    }

    [Test]
    public void FNodeMath_1_plus_2_should_be_a_3() {
        FNodeMath testSubject = new FNodeMath();
        testSubject._Ready();

        //TODO refactor: - doing "testSubject.inputs["Val1"].DefaultValue = 1" will not work because the value is read from the UI here...

        testSubject.inputs["Val1"].UpdateUIFromValue(1f);
        testSubject.inputs["Val2"].UpdateUIFromValue(2f);

        
        Assert.IsEqual(testSubject.outputs["Result"].Get(), 3f);
    }

}

}