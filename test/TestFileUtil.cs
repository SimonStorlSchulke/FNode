using Godot;
using System;


public class TestFileUtil : WAT.Test
{
    [Test]
    public void FileUtil_isAbsolutePath_should_work() {

        FileUtil.IsAbsolutePath("C:/home");
        


        #if(LINUX)
        var pathsToCheck = new string[] { "C:/home", "B" };
        #elif(WINDOWS)
        #endif


        Assert.IsEqual(FileUtil.IsAbsolutePath("/home"), false);
    }

} 


