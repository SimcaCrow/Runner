using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Affdex;
using System.Linq;
using System.IO;
using System.Text;

public class PlatformsGenerationPreExpController : PlatformsGenerationController
{
    private const string dataFolder = "Assets/Resources/PreExp/";
    private const string ext = ".csv";
    private const char sep = ';';
    private string fileName;
    private string filePath;

    private GameObject pauseMenu;
    private PlayerEmotionController emotionController;
    public float minDetection;
    public GameObject indication;
    private ParticleSystem trail;
    private int mode;
    private float startTime;
    private int spendingTime;
    public int delay;

    private float saveSpendingTime;
    private float saveDelay;

    /** 
     *  Initialization
     */
    protected override void Start()
    {
        base.Start();
        pauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        emotionController = this.gameObject.AddComponent<PlayerEmotionController>();
        mode = PlayerPrefs.GetInt("TestMode");
        if (!PlayerPrefs.GetString("PlayerName").Equals("Default")) CreateFile();

        trail = player.GetComponentInChildren<ParticleSystem>();
        trail.Pause();

        startTime = Time.time;
        spendingTime = delay;
        saveDelay = 0.5f;

        if (mode == 1) CoinLevel();
        else if (mode == 2) TrapLevel();
    }

    /** 
     *  Update is called once per frame
     */
    protected override void Update()
    {
        base.Update();
        //If the player is not Default, the avatar is not dead and the pause menu is not display
        if (!PlayerPrefs.GetString("PlayerName").Equals("Default") && player.activeSelf && !pauseMenu.activeSelf) SaveEmotion();
        if (mode == 3) SpeedLevel();
        else if (mode == 4) IndicationLevel();
    }

    /** 
     *  CoinLevel
     */
    void CoinLevel()
    {
        TagProbabilities("trapPlatform", 0);
        TagProbabilities("water", 0);
    }

    /** 
     *  TrapLevel
     */
    void TrapLevel()
    {
        TagProbabilities("coinPlatform", 0);
    }

    /** 
     *  SpeedLevel
     */
    void SpeedLevel()
    {
        if (Time.time - startTime > spendingTime)
        {
            PlayerController p = player.GetComponent<PlayerController>();
            p.ResetSpeed();
            p.startSpeed = Random.Range(3.5f, 7.5f);
            spendingTime += delay;
        }
    }

    /** 
     *  IndicationLevel
     */
    void IndicationLevel()
    {
        if (lastPlatformCreated.GetComponent<PrefabController>().myType != TypeOfPlatform.neutral)
        {
            Vector3 gap = new Vector3(transform.position.x - (TrapGap() - 1) / 2 - lastPlatformCreated.GetComponent<PrefabController>().width / 2, transform.position.y + 0.45f, transform.position.z);
            Instantiate(indication, gap, transform.rotation);
        }
    }

    /**
     * Save the evolution of the emotions
     */
    void SaveEmotion()
    {
        if (File.Exists(filePath))
        {
            string newLine = "";
            if (Time.time - startTime > saveSpendingTime)
            {
                foreach (KeyValuePair<string, float> entry in emotionController.emotions)
                {
                    newLine += entry.Value.ToString() + sep;
                }
                newLine += Time.time.ToString() + '\n';

                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine(newLine);
                writer.Flush();
                writer.Close();

                saveSpendingTime += saveDelay;
            }
        }
        else
        {
            string errorMessage = "ERR! File " + filePath + " does not exists!";
            Debug.Log(errorMessage);
        }
    }

    /**
     * Creation of a new data file
     */
    void CreateFile()
    {
        fileName = PlayerPrefs.GetString("PlayerName") + mode.ToString() + ext;
        filePath = dataFolder + fileName;

        if (!File.Exists(filePath))
        {
            string firstLine = "";
            foreach (KeyValuePair<string, float> entry in emotionController.emotions)
            {
                firstLine += entry.Key + sep;
            }
            firstLine += "Time" + '\n';

            File.Create(filePath).Dispose();
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(firstLine);
            writer.Flush();
            writer.Close();
        }
    }
}
