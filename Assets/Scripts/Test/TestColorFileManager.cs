using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

public class TestColorFileManager : MonoBehaviour
{
    private static readonly string path = "Assets/Resources/Test/colors.csv";
    private static readonly char sep = ';';

    private Color anger = new Color(1, 0, 0, 1);
    private Color surprise = new Color(0, 1, 0, 1);
    private Color joy = new Color(1, 1, 0, 1);
    private Color sadness = new Color(0, 0, 1, 1);

    /** 
     *  Launch of tests
     */
    void Start()
    {
        Debug.Log("Tests : Start...");
        TestListOfPlayers();
        TestEditPlayer();
        TestRemovePlayer();
        TestResetFile();
        TestFindColors();
        Debug.Log("Tests Finished!\n");
    }

    /** 
     *	Unit test of the ListOfPlayers function
     */
    void TestListOfPlayers()
    {
        print("TestListOfPlayers...");
        List<string> playerList;
        PlayerColor newPlayer;
        CreateFile();

        //Empty list
        playerList = ColorFileManager.ListOfPlayers(path);
        Assert.AreEqual(0, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0!"));

        //Add a player
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        playerList = ColorFileManager.ListOfPlayers(path);
        Assert.AreEqual(1, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add an other player
        newPlayer = new PlayerColor("Test2", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        playerList = ColorFileManager.ListOfPlayers(path);
        Assert.AreEqual(2, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Edit the first player
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        playerList = ColorFileManager.ListOfPlayers(path);
        Assert.AreEqual(2, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the EditPlayer function
     */
    void TestEditPlayer()
    {
        print("TestEditPlayer...");
        PlayerColor newPlayer;
        string[] data;
        List<string> trueData;
        CreateFile();

        //Player not in the file
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(1, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add a player
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(2, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Edit the first player
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(2, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Add an other player
        newPlayer = new PlayerColor("Test2", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(3, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the RemovePlayer function
     */
    void TestRemovePlayer()
    {
        print("TestRemovePlayer...");
        PlayerColor newPlayer;
        string[] data;
        List<string> trueData;
        CreateFile();

        //Empty file
        ColorFileManager.RemovePlayer("Test", path);

        //Player not in the file
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        newPlayer = new PlayerColor("Test2", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);

        ColorFileManager.RemovePlayer("Test3", path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(3, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));

        //Player in file
        ColorFileManager.RemovePlayer("Test", path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(2, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the ResetFile function
     */
    void TestResetFile()
    {
        print("TestResetFile...");
        string[] data;
        List<string> trueData;
        CreateFile();

        //Empty file
        DataFileManager.ResetFile(path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(1, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add a player
        PlayerColor newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(2, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Reset the file
        ColorFileManager.ResetFile(path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(1, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Check the first line of the file
        string firstline = "name;angerR;angerG;angerB;surpriseR;surpriseG;surpriseB;joyR;joyG;joyB;sadnessR;sadnessG;sadnessB";
        Assert.AreEqual(firstline, data[0], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Invalid firstline!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the FindColors function
     */
    void TestFindColors()
    {
        print("TestFindColors...");
        Dictionary<string, Color> data;
        PlayerColor newPlayer;
        CreateFile();

        //Empty file
        data = ColorFileManager.FindColors("Test", path);
        Assert.AreEqual(null, data, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Data should not exist"));

        //Add a player
        newPlayer = new PlayerColor("Test", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = ColorFileManager.FindColors("Test", path);
        Assert.AreEqual(anger, data["Anger"], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Wrong color!"));

        //Add an other player
        newPlayer = new PlayerColor("Test", surprise, anger, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        newPlayer = new PlayerColor("Test2", anger, surprise, joy, sadness);
        ColorFileManager.EditPlayer(newPlayer, path);
        data = ColorFileManager.FindColors("Test", path);
        Assert.AreEqual(surprise, data["Anger"], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Wrong color!"));

        DeleteFile();
    }

    /** 
     *	Create a data file for the tests
     */
    void CreateFile()
    {
        if (!File.Exists(path))
            File.Create(path).Dispose();
        DataFileManager.ResetFile(path);
    }

    /** 
     *	Delete the data file created for the tests
     */
    void DeleteFile()
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    /** 
     *	Erased the blank line in a data file
     */
    static List<string> FileWithoutBlank(string[] data)
    {
        List<string> trueData = new List<string>();
        foreach (string line in data)
        {
            if (line.Length > 1)
                trueData.Add(line);
        }
        return trueData;
    }
}
