using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

public class DataFileManager : MonoBehaviour
{
    private const string dataFile = "Assets/Resources/data.csv";
    private const char sep = ';';

    /**
     *  Return the list of all the player in the data file
     */
    public static List<string> ListOfPlayers(string path = dataFile)
    {
        List<string> players = new List<string>();

        if (!File.Exists(path))
        {
            string errorMessage = "ERR! File data does not exists!";
            Debug.Log(errorMessage);
            return new List<string>() { errorMessage };
        }
        else
        {
            string[] data = System.IO.File.ReadAllLines(path);
            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { sep });
                if (row[0].Length > 1)
                {
                    players.Add(row[0]);
                }
            }
            return players;
        }
    }

    /**
     *  Add the data of the new player in the csv file
     */
    public static void AddNewPlayer(PlayerData newPlayer, string path = dataFile)
    {
        newPlayer.Id = NextId(newPlayer.Name, path);

        if (File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path, true);

            string res = "";
            System.Type t = typeof(PlayerData);
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                //print(prop.GetValue(newPlayer, null).ToString());
                res = res + prop.GetValue(newPlayer, null).ToString() + sep;
            }
            res = res.Remove(res.Length - 1);
            res += '\n';
            writer.WriteLine(res);
            writer.Flush();
            writer.Close();
        }
        else
        {
            string errorMessage = "ERR! File " + path + " does not exists!";
            Debug.Log(errorMessage);
        }
    }

    /**
     *  Return the next game id
     */
    public static int NextId(string playerName, string path = dataFile)
    {
        if (File.Exists(path))
        {
            int maxId = 0;
            int tempId = 0;
            string[] data = System.IO.File.ReadAllLines(path);
            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { sep });
                if (row[0].Equals(playerName))
                {
                    if (int.TryParse(row[1], out tempId))
                    {
                        if (tempId > maxId) maxId = tempId;
                    }
                    else
                    {
                        Debug.Log("ERR! Invalid Data");
                        return -1;
                    }
                }
            }
            return maxId + 1;
        }
        else
        {
            string errorMessage = "ERR! File " + path + " does not exists!";
            Debug.Log(errorMessage);
            return -1;
        }
    }

    /**
     *  Remove the data of a player depending of his name
     */
    public static void RemovePlayer(string playerName, string path = dataFile)
    {
        List<string> playersData = new List<string>();

        if (!File.Exists(path))
        {
            string errorMessage = "ERR! File " + path + " does not exists!";
            Debug.Log(errorMessage);
        }
        else
        {
            string[] data = System.IO.File.ReadAllLines(path);

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { sep });
                if (row[0].Length > 1)
                {
                    if (!row[0].Equals(playerName))
                    {
                        playersData.Add(data[i]);
                    }

                }
            }

            ResetFile(path);

            StringBuilder sb = new StringBuilder();
            foreach (string playerData in playersData)
            {
                if (playersData.LastOrDefault().Equals(playerData))
                {
                    sb.Append(playerData);
                }
                else
                {
                    sb.AppendLine(playerData);
                }
            }
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(sb);
            writer.Flush();
            writer.Close();
        }
    }

    /**
     *  Erase the content of the csv file, and write the first line (description of the data)
     */
    public static void ResetFile(string path = dataFile)
    {
        if (File.Exists(path))
        {
            List<string> list = typeof(PlayerData).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Select(field => field.Name).ToList();
            string firstLine = "";
            foreach (string attribut in list)
                firstLine = firstLine + attribut + sep;
            firstLine = firstLine.Remove(firstLine.Length - 1);
            firstLine += '\n';
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(firstLine);
            StreamWriter writer = new StreamWriter(path, false);
            //writer.WriteLine(sb);
            writer.WriteLine(firstLine);
            writer.Flush();
            writer.Close();
        }
        else
        {
            string errorMessage = "ERR! File " + path + " does not exists!";
            Debug.Log(errorMessage);
        }
    }

    /**
     *  Return the data ratio for all the trap platforms
     */
    public static Dictionary<string, float> FindData(string playerName, string path = dataFile)
    {
        if (!File.Exists(path))
        {
            string errorMessage = "ERR! File data does not exists!";
            Debug.Log(errorMessage);
            return null;
        }
        else
        {
            string[] data = System.IO.File.ReadAllLines(path);
            List<string> playerData = new List<string>();
            //skip the first line (description of the data) and the last one (empty line)
            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] row = data[i].Split(new char[] { sep });
                if (row[0].Equals(playerName))
                {
                    playerData.Add(data[i]);
                }
            }
            if (playerData.Count == 0)
            {
                Debug.Log("ERR! Player does not exists!");
                return null;
            }
            else
            {
                Dictionary<string, float> playerRatio = new Dictionary<string, float>() { { "platform5", 0 }, { "platform6", 0 }, { "platform7", 0 }, { "platform10", 0 } };
                int[] playerCount = new int[8];
                int temp;

                foreach (string game in playerData)
                {
                    string[] row = game.Split(new char[] { sep });
                    if (row.Length < 11)
                    {
                        Debug.Log("ERR! Invalid Data");
                        return null;
                    }
                    for (int i = 3; i < 11; i++)
                    {
                        if (int.TryParse(row[i], out temp) && int.Parse(row[2]) == 0) playerCount[i - 3] = playerCount[i - 3] + temp;
                        else
                        {
                            Debug.Log("ERR! Invalid Data");
                            return null;
                        }
                    }
                }

                playerRatio["platform5"] = (playerCount[0] == 0) ? -1 : 1f * playerCount[4] / playerCount[0];
                playerRatio["platform6"] = (playerCount[1] == 0) ? -1 : 1f * playerCount[5] / playerCount[1];
                playerRatio["platform7"] = (playerCount[2] == 0) ? -1 : 1f * playerCount[6] / playerCount[2];
                playerRatio["platform10"] = (playerCount[3] == 0) ? -1 : 1f * playerCount[7] / playerCount[3];

                return playerRatio;
            }
        }
    }
}
