using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

public class TestDataFileManager : MonoBehaviour
{
    private static readonly string path = "Assets/Resources/Test/data.csv";
    private static readonly char sep = ';';

    /** 
     *  Launch of tests
     */
    void Start()
    {
        Debug.Log("Tests : Start...");
        TestListOfPlayers();
        TestAddNewPlayer();
        TestRemovePlayer();
        TestResetFile();
        TestFindData();
        Debug.Log("Tests Finished!\n");
    }

    /** 
     *	Unit test of the ListOfPlayers function
     */
    void TestListOfPlayers()
    {
        print("TestListOfPlayers...");
        List<string> playerList;
        PlayerData newPlayer;
        CreateFile();

        //Empty list
        playerList = DataFileManager.ListOfPlayers(path);
        Assert.AreEqual(0, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0!"));

        //Add a player
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        playerList = DataFileManager.ListOfPlayers(path);
        Assert.AreEqual(1, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add an other player
        newPlayer = new PlayerData("Test2", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        playerList = DataFileManager.ListOfPlayers(path);
        Assert.AreEqual(2, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Add an other game for the first player
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        playerList = DataFileManager.ListOfPlayers(path);
        Assert.AreEqual(3, playerList.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the AddNewPlayer function
     */
    void TestAddNewPlayer()
    {
        print("TestAddNewPlayer...");
        int id;
        PlayerData newPlayer;
        CreateFile();

        //Player not in the file
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(1, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add a player
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(2, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));
        id = DataFileManager.NextId("Test2", path);
        Assert.AreEqual(1, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Add an other game for the player
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 0, 1, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(3, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));

        //Add an other player
        newPlayer = new PlayerData("Test2", 1, 2, 3, 4, 0, 0, 0, 1, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(3, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));
        id = DataFileManager.NextId("Test2", path);
        Assert.AreEqual(2, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the RemovePlayer function
     */
    void TestRemovePlayer()
    {
        print("TestRemovePlayer...");
        int id;
        PlayerData newPlayer;
        CreateFile();

        //Empty file
        DataFileManager.RemovePlayer("Test", path);

        //Player not in the file
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 0, 1, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        newPlayer = new PlayerData("Test2", 1, 2, 3, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);

        DataFileManager.RemovePlayer("Test3", path);
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(3, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 3!"));
        id = DataFileManager.NextId("Test2", path);
        Assert.AreEqual(2, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));
        id = DataFileManager.NextId("Test3", path);
        Assert.AreEqual(1, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Player in file
        DataFileManager.RemovePlayer("Test", path);
        id = DataFileManager.NextId("Test", path);
        Assert.AreEqual(1, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));
        id = DataFileManager.NextId("Test2", path);
        Assert.AreEqual(2, id, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

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
        PlayerData newPlayer = new PlayerData("Test", 1, 2, 3, 4, 0, 0, 0, 1, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(2, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 2!"));

        //Reset the file
        DataFileManager.ResetFile(path);
        data = System.IO.File.ReadAllLines(path);
        trueData = FileWithoutBlank(data);
        Assert.AreEqual(1, trueData.Count, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 1!"));

        //Check the first line of the file
        string firstline = "name;id;randomMode;platform5;platform6;platform7;platform10;spikes;catcher;evilBlock;evilSaw";
        Assert.AreEqual(firstline, data[0], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Invalid firstline!"));

        DeleteFile();
    }

    /** 
     *	Unit test of the FindData function
     */
    void TestFindData()
    {
        print("TestFindData...");
        Dictionary<string, float> data;
        PlayerData newPlayer;
        CreateFile();

        //Empty file
        data = DataFileManager.FindData("Test", path);
        Assert.AreEqual(null, data, string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Data should not exist"));

        //Add a player
        newPlayer = new PlayerData("Test", 1, 2, 4, 4, 0, 0, 1, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        data = DataFileManager.FindData("Test", path);
        Assert.AreEqual(0.25f, data["platform7"], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0.25!"));

        //Add an other player
        newPlayer = new PlayerData("Test", 2, 1, 12, 3, 0, 1, 0, 0, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        newPlayer = new PlayerData("Test2", 1, 2, 3, 4, 0, 0, 0, 1, 0);
        DataFileManager.AddNewPlayer(newPlayer, path);
        data = DataFileManager.FindData("Test", path);
        Assert.AreEqual(0.0625f, data["platform7"], string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(255), (byte)(0), (byte)(0), "...FAILED : Value should be 0.0625!"));

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
