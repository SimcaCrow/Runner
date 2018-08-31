using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public class TestTextFileController : TextFileController
{
    private static readonly string path = @"Assets/Resources/Test/informations.txt";
    private static readonly char sep = '=';

    /** 
     *  Launch of tests
     */
    void Start()
    {
        Debug.Log("Tests : Start...");
        TestTextReader();
        TestTextParser();
        TestStringToFloatToInt();
        Debug.Log("Tests Finished!\n");
    }

    /** 
     *	Unit test of the TextReader function
     */
    private void TestTextReader()
    {
        print("TestTextLoader...");

        // File does not exist
        Assert.AreEqual(TextReader(path), "", string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : empty string has to be set in case of error!"));

        // Empty File
        CreateFile();
        Assert.AreEqual(TextReader(path), "", string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Text should be empty!"));

        // Not empty File
        string text = "test";
        WriteFile(text);
        Assert.AreEqual(TextReader(path), text, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Text should be 'test'!"));

        ClearFile();
        DeleteFile();
    }

    /** 
     *	Unit test of the TextParser function
     */
    private void TestTextParser()
    {
        print("TestTextParser...");
        bodyOutput.Clear();
        int entryNumber = bodyOutput.Count;

        // Empty text
        TextParser("");
        Assert.AreEqual(bodyOutput.Count, entryNumber, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Dictinnary should not cointain more data!"));

        // One line text
        TextParser("Test=0;");
        Assert.AreEqual(bodyOutput.ContainsKey("Test"), true, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be True !"));
        Assert.AreEqual(bodyOutput["Test"], 0, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0 !"));
        bodyOutput.Clear();

        // Multi lines text
        TextParser("Test_1=1;Test_2=2;");
        Assert.AreEqual(bodyOutput.ContainsKey("Test_1"), true, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be True !"));
        Assert.AreEqual(bodyOutput["Test_1"], 1, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED :Value should be 1 !"));
        Assert.AreEqual(bodyOutput.ContainsKey("Test_2"), true, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be True !"));
        Assert.AreEqual(bodyOutput["Test_2"], 2, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED :Value should be 2 !"));
        bodyOutput.Clear();

        // Text without correct format
        entryNumber = bodyOutput.Count;
        TextParser("Test_3=3;Test_4:4;");
        Assert.AreEqual(bodyOutput.ContainsKey("Test_3"), true, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be True !"));
        Assert.AreEqual(bodyOutput["Test_3"], 3, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED :Value should be 1 !"));
        Assert.AreEqual(bodyOutput.ContainsKey("Test_4"), false, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be False !"));
        Assert.AreEqual(bodyOutput.Count, entryNumber + 1, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1 !"));
        bodyOutput.Clear();

        entryNumber = bodyOutput.Count;
        TextParser("Test_5=5,Test_6=6;");
        Assert.AreEqual(bodyOutput.ContainsKey("Test_5"), true, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be False !"));
        Assert.AreEqual(bodyOutput.ContainsKey("Test_6"), false, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be False !"));
        Assert.AreEqual(bodyOutput.Count, entryNumber + 1, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0 !"));
        bodyOutput.Clear();

        // modified data
    }

    /** 
     *	Unit test of the StringToFloatToInt function
     */
    private void TestStringToFloatToInt()
    {
        print("TestStringToFloatToInt...");

        // Int string to int
        Assert.AreEqual(StringToFloatToInt("1"), 1, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));
        Assert.AreEqual(StringToFloatToInt("0010"), 10, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 10!"));

        // Float string to int
        Assert.AreEqual(StringToFloatToInt("0.0"), 0, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0!"));
        Assert.AreEqual(StringToFloatToInt("1.5"), 2, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        // String to int with incorrect format
        Assert.AreEqual(StringToFloatToInt("test"), 0, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : 0 has to be set in case of error!"));
    }

    /** 
     *	Create an empty file
     */
    private void CreateFile()
    {
        if (!File.Exists(path))
            File.Create(path).Dispose();
    }

    /** 
     *	Write data on the file
     */
    private void WriteFile(string text)
    {
        if (File.Exists(path))
            File.WriteAllText(path, text);
    }

    /** 
     *	Read data on the file
     */
    private string ReadFile()
    {
        string text = "";
        if (File.Exists(path))
            text = File.ReadAllText(path);
        return text;
    }

    /** 
     *	Clear data on the file
     */
    private void ClearFile()
    {
        if (File.Exists(path))
            File.WriteAllText(path, string.Empty);
    }

    /** 
     *	Delete the file
     */
    private void DeleteFile()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}
