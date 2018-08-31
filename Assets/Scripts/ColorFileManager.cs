using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

public class ColorFileManager : MonoBehaviour
{
    private const string dataFile = "Assets/Resources/colors.csv";
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
     *  Edit the data of a player. If the player does not already exist, add him in the data file
     */
    public static void EditPlayer(PlayerColor newPlayer, string path = dataFile)
    {
        List<string> players = new List<string>();
        players = ListOfPlayers(path);
        bool inList = false;

        foreach (string playerName in players)
            if (newPlayer.Name.Equals(playerName)) inList = true;

        if (!inList) AddNewPlayer(newPlayer, path);
        else
        {
            RemovePlayer(newPlayer.Name, path);
            AddNewPlayer(newPlayer, path);
        }
    }

    /**
     *  Add the data of the new player in the csv file
     */
    static void AddNewPlayer(PlayerColor newPlayer, string path = dataFile)
    {
        if (File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path, true);

            string res = "";
            System.Type t = typeof(PlayerColor);
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
     *  Add the data of the new player in the csv file
     */
    public static Dictionary<string, Color> FindColors(string playerName, string path = dataFile)
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
            float[] values = new float[12];
            //skip the first line (description of the data) and the last one (empty line)
            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { sep });
                if (row[0].Equals(playerName))
                {
                    for (int j = 1; j < 13; j++)
                    {
                        if (float.TryParse(row[j], out values[j - 1])) continue;
                        else
                        {
                            Debug.Log("ERR! Invalid Data");
                            return null;
                        }
                    }

                    Color[] colors = new Color[4];
                    for (int j = 0; j < 4; j++)
                    {
                        colors[j].r = values[j * 3];
                        colors[j].g = values[j * 3 + 1];
                        colors[j].b = values[j * 3 + 2];
                        colors[j].a = 1;
                    }
                    Dictionary<string, Color> playerColor = new Dictionary<string, Color>();
                    playerColor.Add("Anger", colors[0]);
                    playerColor.Add("Surprise", colors[1]);
                    playerColor.Add("Joy", colors[2]);
                    playerColor.Add("Sadness", colors[3]);
                    return playerColor;
                }
            }

            Debug.Log("ERR! Player does not exists!");
            return null;
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
            List<string> list = typeof(PlayerColor).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Select(field => field.Name).ToList();
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
}
