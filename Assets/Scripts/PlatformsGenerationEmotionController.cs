using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Affdex;
using System.Linq;
using System.IO;
using System.Text;

public class PlatformsGenerationEmotionController : PlatformsGenerationController
{
    public float minDetection;
    private GameObject pauseMenu;
    private PlayerEmotionController emotionController;
    private MusicController musicController;
    private ParticleSystem trail;
    public GameObject indication;
    private string emotionName;

    private Renderer playerRenderer;
    private float timeLeft;
    private Color targetColor;
    private Color initialColor;

    private Dictionary<string, Color> playerColors;

    private delegate void Feedback();
    private Dictionary<string, Feedback> emotionFeedback;

    private const string dataFolder = "Assets/Resources/PreExp/";
    private const string ext = ".csv";
    private const char sep = ';';

    /** 
     *  Initialization
     */
    protected override void Start()
    {
        base.Start();
        pauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        emotionController = this.gameObject.AddComponent<PlayerEmotionController>();
        musicController = FindObjectOfType<MusicController>();
        trail = player.GetComponentInChildren<ParticleSystem>();
        trail.Pause();
        emotionName = "null";
        playerRenderer = player.GetComponent<Renderer>();
        targetColor = playerRenderer.material.color;
        initialColor = playerRenderer.material.color;

        //Anger and Surprise increase the number of traps, Joy and Sadness increase the number of coins
        emotionFeedback = new Dictionary<string, Feedback>() { { "Anger", FeedbackTrap }, { "Surprise", FeedbackCoin }, { "Joy", FeedbackCoin }, { "Sadness", FeedbackTrap } };

        playerColors = new Dictionary<string, Color>() { { "Anger", new Color(1, 0, 0) }, { "Surprise", new Color(0, 1, 0) }, { "Joy", new Color(1, 1, 0) }, { "Sadness", new Color(0, 0, 1) } };
        SetPlayerColors();

        //  WARNING : OBSOLETE, USED FOR THE 4 TEST LEVELS SYSTEM
        /*
        if (PlayerPrefs.GetInt("RandomMode") == 0)
        {
            SetPlayerColors();
            ChangeEmotionFeedbak();
        }
        else if (PlayerPrefs.GetInt("RandomMode") == 1) ChangeEmotionFeedbakRandom();
        */
        // WARNING : END
    }

    /** 
     *  Update is called once per frame
     */
    protected override void Update()
    {
        base.Update();
        if (player.activeSelf && !pauseMenu.activeSelf)
        {
            if (PlayerPrefs.GetInt("RandomMode") != 1) CheckEmotion();
        }
        else if (!player.activeSelf || pauseMenu.activeSelf)
        {
            musicController.ResetMusic();
        }
    }

    /** 
     *  Find the colors of the player
     */
    void SetPlayerColors()
    {
        Dictionary<string, Color> temp = ColorFileManager.FindColors(PlayerPrefs.GetString("PlayerName"));
        if (temp != null)
        {
            playerColors = temp;
        }
    }

    /**
     *  Check if an emotion is detected 
     */
    void CheckEmotion()
    {
        emotionName = emotionController.EmotionDetected(minDetection);
        musicController.SwitchMusic(emotionName);
        if (string.Compare(emotionName, "null") == 0)
        {
            ResetEmotion();
        }
        else
        {
            Color emotionColor = new Color(0f, 0f, 0f, 0f);
            trail.Play();
            switch (emotionName)
            {
                case "currentAnger":
                    emotionColor = playerColors["Anger"];
                    if (PlayerPrefs.GetInt("RandomMode") == 0) emotionFeedback["Anger"]();
                    break;
                case "currentSurprise":
                    emotionColor = playerColors["Surprise"];
                    if (PlayerPrefs.GetInt("RandomMode") == 0) emotionFeedback["Surprise"]();
                    break;
                case "currentJoy":
                    emotionColor = playerColors["Joy"];
                    if (PlayerPrefs.GetInt("RandomMode") == 0) emotionFeedback["Joy"]();
                    break;
                case "currentSadness":
                    emotionColor = playerColors["Sadness"];
                    if (PlayerPrefs.GetInt("RandomMode") == 0) emotionFeedback["Sadness"]();
                    break;
                default:
                    break;
            }
            ChangeColor(emotionColor, playerRenderer);
            ChangeElementsColor(emotionColor);
        }
    }

    /**
     *  Increase the probability of the platform with a trap
     */
    void FeedbackTrap()
    {
        TagProbabilities("trapPlatform", 180);
    }

    /**
     *  Reset the speed of the player
     */
    void FeedbackSpeed()
    {
        player.GetComponent<PlayerController>().ResetSpeed();
    }

    /**
     *  Increase the probability of the platform with a coin
     */
    void FeedbackCoin()
    {
        TagProbabilities("coinPlatform", 180);
    }

    /**
     *  Spawn an indication before the an non-neutral platform
     */
    void FeedbackIndication()
    {
        if (lastPlatformCreated.GetComponent<PrefabController>().myType != TypeOfPlatform.neutral)
        {
            Vector3 gap = new Vector3(transform.position.x - (TrapGap() - 1) / 2 - lastPlatformCreated.GetComponent<PrefabController>().width / 2, transform.position.y + 0.45f, transform.position.z);
            Instantiate(indication, gap, transform.rotation);
        }
    }

    /**
     *  Reset the emotion feedbacks if no emotion is detected
     */
    void ResetEmotion()
    {
        musicController.ResetMusic();
        ChangeColor(initialColor, playerRenderer);
        ChangeElementsColor(initialColor);
        trail.Pause();
        trail.Clear();
        ResetWeight();
    }

    /** 
     *  Change the color of the character 
     */
    void ChangeColor(Color emotionColor, Renderer targetRenderer)
    {
        if (timeLeft <= Time.deltaTime)
        {
            targetRenderer.material.color = targetColor;
            targetColor = emotionColor;
            timeLeft = 1.0f;
        }
        else
        {
            targetRenderer.material.color = Color.Lerp(targetRenderer.material.color, targetColor, Time.deltaTime / timeLeft);
            timeLeft -= Time.deltaTime;
        }
    }

    /** 
     *  Change the color of the elements
     */
    void ChangeElementsColor(Color emotionColor)
    {
        GameObject[] elements;
        elements = GameObject.FindGameObjectsWithTag("element");
        foreach (GameObject element in elements)
        {
            ChangeColor(emotionColor, element.GetComponent<Renderer>());
        }
    }

    /** 
     *  Instantiate the new platform of the level
     */
    protected override void SpawnPlatform(int platformWidth, int platformHeight)
    {
        int n = 1;
        bool firstPlatformsSpawn = lastPlatformCreated.Equals(firstPlatforms);
        if (firstPlatformsSpawn) n = TrapGap();
        else platformName = lastPlatformCreated.name;
        for (int i = 0; i < n; i++)
        {
            transform.position = new Vector3(transform.position.x + platformWidth, transform.position.y + platformHeight, transform.position.z);
            Instantiate(lastPlatformCreated, transform.position, transform.rotation);
            heightLevel = heightLevel + platformHeight;
            SpawnBackgroundBlocks(platformWidth, platformHeight);
            if (firstPlatformsSpawn && string.Compare(emotionName, "null") != 0) SpawnElement();
        }
    }

    // WARNING : OBSOLETE, USED FOR THE 4 TEST LEVELS SYSTEM

    /*
     * Change the link between emotion and feedback randomly
     */
     /*
    void ChangeEmotionFeedbakRandom()
    {
        List<Feedback> feedbackList = new List<Feedback>() { FeedbackTrap, FeedbackSpeed, FeedbackCoin, FeedbackIndication };
        System.Random rnd = new System.Random();
        int n = feedbackList.Count;
        while (n > 1)
        {
            int k = rnd.Next(n--);
            Feedback temp = feedbackList[n];
            feedbackList[n] = feedbackList[k];
            feedbackList[k] = temp;
        }
        emotionFeedback["Anger"] = feedbackList[0];
        emotionFeedback["Surprise"] = feedbackList[1];
        emotionFeedback["Joy"] = feedbackList[2];
        emotionFeedback["Sadness"] = feedbackList[3];
    }
    */

    /*
     * Change the link between emotion and feedback according to the 4 data files
     */
     /*
    void ChangeEmotionFeedbak()
    {
        if (File.Exists(dataFolder + PlayerPrefs.GetString("PlayerName") + "1" + ext)
            && File.Exists(dataFolder + PlayerPrefs.GetString("PlayerName") + "2" + ext)
            && File.Exists(dataFolder + PlayerPrefs.GetString("PlayerName") + "3" + ext)
            && File.Exists(dataFolder + PlayerPrefs.GetString("PlayerName") + "4" + ext))
        {
            Dictionary<string, float> coinTest = EmotionInformation(dataFolder + PlayerPrefs.GetString("PlayerName") + "1" + ext);
            Dictionary<string, float> trapTest = EmotionInformation(dataFolder + PlayerPrefs.GetString("PlayerName") + "2" + ext);
            Dictionary<string, float> speedTest = EmotionInformation(dataFolder + PlayerPrefs.GetString("PlayerName") + "3" + ext);
            Dictionary<string, float> indicationTest = EmotionInformation(dataFolder + PlayerPrefs.GetString("PlayerName") + "4" + ext);

            Dictionary<string, Dictionary<string, float>> allDico = new Dictionary<string, Dictionary<string, float>>() { { "coinTest", coinTest }, { "trapTest", trapTest }, { "speedTest", speedTest }, { "indicationTest", indicationTest } };
            Dictionary<string, Feedback> emotionLevel = new Dictionary<string, Feedback>() { { "coinTest", FeedbackCoin }, { "trapTest", FeedbackTrap }, { "speedTest", FeedbackSpeed }, { "indicationTest", FeedbackIndication } };

            Dictionary<string, Feedback> test = new Dictionary<string, Feedback>();

            string key;
            key = BestEmotion(allDico, "Anger");
            allDico.Remove(key);
            test.Add("Anger", emotionLevel[key]);

            key = BestEmotion(allDico, "Sadness");
            allDico.Remove(key);
            test.Add("Sadness", emotionLevel[key]);

            key = BestEmotion(allDico, "Surprise");
            allDico.Remove(key);
            test.Add("Surprise", emotionLevel[key]);

            key = BestEmotion(allDico, "Joy");
            allDico.Remove(key);
            test.Add("Joy", emotionLevel[key]);

            emotionFeedback = test;
        }
    }
    */

    /*
     * 
     */
     /*
    string BestEmotion(Dictionary<string, Dictionary<string, float>> allDico, string emotion)
    {
        float max = -1;
        string key = "";
        foreach (KeyValuePair<string, Dictionary<string, float>> dico in allDico)
        {
            foreach (KeyValuePair<string, float> entry in dico.Value)
            {
                if (entry.Key.Equals(emotion))
                {
                    if (entry.Value > max)
                    {
                        max = entry.Value;
                        key = dico.Key;
                    }
                }
            }
        }
        return key;
    }
    */

    /*
     *  Return the information of the emotion evolutions during a test level
     */
     /*
    Dictionary<string, float> EmotionInformation(string path)
    {
        if (File.Exists(path))
        {
            string[] data;
            List<string> trueData;
            data = System.IO.File.ReadAllLines(path);
            trueData = FileWithoutBlank(data);
            trueData.RemoveAt(0);
            Dictionary<string, float> emotionInfo = new Dictionary<string, float>() { { "Anger", EmotionAverage(trueData, 0) }, { "Surprise", EmotionAverage(trueData, 1) }, { "Joy", EmotionAverage(trueData, 2) }, { "Sadness", EmotionAverage(trueData, 3) } };
            return emotionInfo;
        }
        else
        {
            string errorMessage = "ERR! File data does not exists!";
            Debug.Log(errorMessage);
            return null;
        }
    }
    */

    /*
     *  Compute the average of all the values associated to a particular emotion, in the given file
     */
     /*
    float EmotionAverage(List<string> data, int index)
    {
        int n = 0;
        float total = 0;
        foreach (string line in data)
        {
            string[] row = line.Split(new char[] { sep });
            float res = 0;
            if (float.TryParse(row[index], out res))
            {
                total += res;
                n++;
            }
            else continue;
        }
        if (n > 0) return total / n;
        else return -1;
    }
    */

    /*
     *  Remove the blank line
     */
     /*
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
    */

    // WARNING : END
}


