using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class TextFileController : MonoBehaviour
{
    private static string path = @"Assets\Resources\informations.txt";
    private string textFileData;
    protected Dictionary<string, int> bodyOutput;

    /** 
     *  Initialization
     */
    void Awake()
    {
        bodyOutput = new Dictionary<string, int>();
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        textFileData = RemoveLineBreakAndSpace(TextReader(path));
        TextParser(textFileData);
    }

    /** 
     *  Read the text in the filePath
     */
    protected string TextReader(string filePath)
    {
        string text = "";
        try
        {
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    text = reader.ReadToEnd();
                }
            }
        }
        catch (FileNotFoundException ioEx)
        {
            Console.WriteLine("File not find:" + ioEx.Message);
        }
        return text;
    }

    /** 
     *  Parse the text variable into the bodyOutput dictionnary
     */
    protected void TextParser(string text)
    {
        try
        {
            string[] values = text.Split(';');
            foreach (var value in values)
            {
                if (value != null)
                {
                    string[] data = value.Split('=');
                    if (bodyOutput.ContainsKey(data[0]))
                    {
                        bodyOutput[data[0]] = StringToFloatToInt(data[1]);
                    }
                    else
                    {
                        bodyOutput.Add(data[0], StringToFloatToInt(data[1]));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The text could not be parse:" + e.Message);
        }
    }

    /** 
     *  Remove line break and space from a textfile
     */
    private string RemoveLineBreakAndSpace(string text)
    {
        text = text.Replace(Environment.NewLine, string.Empty);
        text = text.Replace(" ", string.Empty);
        return text;
    }

    /** 
     *  Convert string into int
     */
    protected static int StringToFloatToInt(string text)
    {
        int value = 0;
        try
        {
            value = (int)Math.Round(Convert.ToDouble(text));
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot convert this string to int : " + e.Message);
        }
        return value;
    }

    /** 
     *  Return the number of calories lost read in the dictionnary
     */
    public int CaloriesLost()
    {
        if (bodyOutput.ContainsKey("Calories"))
        {
            return bodyOutput["Calories"];
        }
        return 0;
    }
}
