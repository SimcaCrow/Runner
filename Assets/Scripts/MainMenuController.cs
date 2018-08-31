using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public AudioSource selectSound;

    public Sprite RedEmotionImage;
    public Sprite GreenEmotionImage;
    public Sprite RedBodyImage;
    public Sprite GreenBodyImage;

    private GameObject mainMenu;
    private GameObject coinAnimation;
    private GameObject bodyMenu;
    private GameObject emotionMenu;
    private GameObject playerMenu;
    private GameObject infoMenu;
    private GameObject highScoreMenu;

    private GameObject mainMenuButton;
    private GameObject randomLevelButton;
    private GameObject intelligentLevelButton;
    private GameObject playButton;

    private GameObject dropdownElements;
    private Dropdown dropdown;

    private PlayerSettingsController playerSettingsManager;

    /** 
     *	Initialization
     */
    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        coinAnimation = GameObject.Find("Coin");
        bodyMenu = GameObject.Find("Canvas").transform.Find("BodyMenu").gameObject;
        emotionMenu = GameObject.Find("Canvas").transform.Find("EmotionMenu").gameObject;
        playerMenu = GameObject.Find("Canvas").transform.Find("PlayerMenu").gameObject;
        infoMenu = GameObject.Find("Canvas").transform.Find("InfoMenu").gameObject;
        highScoreMenu = GameObject.Find("Canvas").transform.Find("HighScoreMenu").gameObject;

        mainMenuButton = GameObject.Find("Canvas").transform.Find("MainMenuButton").gameObject;
        randomLevelButton = GameObject.Find("Canvas").transform.Find("RandomLevelButton").gameObject;
        intelligentLevelButton = GameObject.Find("Canvas").transform.Find("IntelligentLevelButton").gameObject;
        playButton = GameObject.Find("Canvas").transform.Find("PlayButton").gameObject;

        dropdownElements = GameObject.Find("Canvas").transform.Find("Dropdown").gameObject;
        dropdown = dropdownElements.GetComponent<Dropdown>();

        playerSettingsManager = FindObjectOfType<PlayerSettingsController>();

        InitPlayerPrefs();
    }

    /** 
     *	Initialize PlayerPrefs
     */
    private void InitPlayerPrefs()
    {
        PlayerPrefs.SetInt("CaloriesToLost", 0);
        PlayerPrefs.SetInt("Timer", 0);
        PlayerPrefs.SetInt("PreviousTime", 0);
        PlayerPrefs.SetInt("Speed", 0);
        PlayerPrefs.SetInt("BodyMode", 1);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("TestMode", 0);
        playerSettingsManager.SetPlayerName(playerSettingsManager.GetDefaultPlayer());
    }

    /** 
     *	Change mode to Body mode : Physical training
     */
    public void SelectBodyMode()
    {
        if (PlayerPrefs.GetInt("BodyMode") != 1)
        {
            PlayerPrefs.SetInt("BodyMode", 1);
            GameObject.Find("BodyButton").GetComponent<Image>().sprite = GreenBodyImage;
            GameObject.Find("EmotionButton").GetComponent<Image>().sprite = RedEmotionImage;
        }
    }

    /** 
     *	Change mode to Emotion mode : Mental Training
     */
    public void SelectEmotionMode()
    {
        if (PlayerPrefs.GetInt("BodyMode") == 1)
        {
            PlayerPrefs.SetInt("BodyMode", 0);
            GameObject.Find("EmotionButton").GetComponent<Image>().sprite = GreenEmotionImage;
            GameObject.Find("BodyButton").GetComponent<Image>().sprite = RedBodyImage;
        }
    }

    /** 
     *	Load the correct mode according to PlayerPrefs
     */
    public void LoadGameMenu()
    {
        DisabledMainMenu();
        playButton.SetActive(true);
        dropdownElements.SetActive(true);
        randomLevelButton.SetActive(true);
        intelligentLevelButton.SetActive(true);
        playerSettingsManager.UpdateDropdown();
        if (PlayerPrefs.GetInt("BodyMode") == 1)
        {
            bodyMenu.SetActive(true);
        }
        else
        {
            ResetPlayerPrefs();
            emotionMenu.SetActive(true);
        }
    }

    /** 
     *	Launch the level scene
     */
    public void StartGame()
    {
        selectSound.Play();
        playerSettingsManager.SetPlayerName(dropdown.options[dropdown.value].text);
        SceneManager.LoadScene("Level");
    }

    /** 
     *	Launch the tutorial
     */
    public void StartTutorial()
    {
        selectSound.Play();
        SceneManager.LoadScene("Tutorial");
    }

    /** 
     *	Display the player settings screen
     */
    public void PlayerScreen()
    {
        DisabledMainMenu();
        playerMenu.SetActive(true);
        playerSettingsManager.EnabledPlayerMenu();
    }

    /** 
     *	Display the information screen
     */
    public void InfoScreen()
    {
        DisabledMainMenu();
        infoMenu.SetActive(true);
    }

    /** 
     *	Display the high score screen
     */
    public void HighScoreScreen()
    {
        DisabledMainMenu();
        highScoreMenu.SetActive(true);
        GameObject.Find("HighScoreValueText").GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    /** 
     *	Disabled main menu elements
     */
    private void DisabledMainMenu()
    {
        selectSound.Play();
        mainMenu.SetActive(false);
        coinAnimation.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    /** 
     *	Exit the game
     */
    public void ExitGame()
    {
        selectSound.Play();
        Application.Quit();
    }

    /** 
     *	Return to the main menu screen and disabled other elements
     */
    public void BackToMainMenu()
    {
        selectSound.Play();
        mainMenuButton.SetActive(false);
        playButton.SetActive(false);
        dropdownElements.SetActive(false);
        randomLevelButton.SetActive(false);
        intelligentLevelButton.SetActive(false);

        bodyMenu.SetActive(false);
        emotionMenu.SetActive(false);
        playerMenu.SetActive(false);
        playerSettingsManager.DisabledPlayersSettingsScreen();
        infoMenu.SetActive(false);
        highScoreMenu.SetActive(false);

        mainMenu.SetActive(true);
        coinAnimation.SetActive(true);
    }

    /** 
     *	Run the random or the intelligent mode
     */
    public void RunRandomMode(int number)
    {
        PlayerPrefs.SetInt("TestMode", 0);
        PlayerPrefs.SetInt("RandomMode", number);
        ResetPlayerPrefs();
        StartGame();
    }

    /** 
     *	Run the test number
     */
    public void RunEmotionTest(int number)
    {
        PlayerPrefs.SetInt("TestMode", number);
        ResetPlayerPrefs();
        StartGame();
    }

    /** 
     *	Set PlayerPrefs to default
     */
    private void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("CaloriesToLost", 0);
        PlayerPrefs.SetInt("Timer", 0);
        PlayerPrefs.SetInt("PreviousTime", 0);
        PlayerPrefs.SetInt("Speed", 0);
    }
}
