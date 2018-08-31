using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenuController : MonoBehaviour
{
    public AudioSource highscoreSound;
    public AudioSource selectSound;

    private Text deathMenuScore;
    private Text deathMenuHighScore;
    private Text caloriesGoalText;

    private GameObject deathMenu;

    private GameController gameManager;
    private ScoreController scoreManager;
    private CoinsController coinsManager;
    private CaloriesController caloriesManager;
    private TimeController timeManager;

    private int timer;

    /** 
     *  Initialization
     */
    void Start()
    {
        deathMenu = GameObject.Find("DeathMenu").gameObject;
        deathMenuScore = GameObject.Find("DeathMenuScore").GetComponent<Text>();
        deathMenuHighScore = GameObject.Find("DeathMenuHighScore").GetComponent<Text>();
        caloriesGoalText = GameObject.Find("CaloriesGoalText").GetComponent<Text>();

        gameManager = FindObjectOfType<GameController>();
        scoreManager = FindObjectOfType<ScoreController>();
        coinsManager = FindObjectOfType<CoinsController>();
        caloriesManager = FindObjectOfType<CaloriesController>();
        timeManager = FindObjectOfType<TimeController>();

        timer = PlayerPrefs.GetInt("Timer");

        if (timer > 0)
        {
            timeManager.SetIsDead(true);
            timeManager.FixPreviousTime();
        }

        DisplayScoring();
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        if (deathMenu.activeSelf && Input.GetKey(KeyCode.R))
        {
            Restart();
        }
    }

    /** 
     *  Display all scores in the deathMenu
     */
    private void DisplayScoring()
    {
        int score = scoreManager.GetScore();
        int totalScore = score + coinsManager.CoinsScoring();

        if (PlayerPrefs.GetInt("BodyMode") == 1)
        {
            int caloriesLost = caloriesManager.GetCaloriesLost();
            int caloriesToLost = PlayerPrefs.GetInt("CaloriesToLost");

            deathMenuScore.text = System.String.Format("Distance : {0}m\nCoins : {1}\nMultiplier : {2}x\nTotal Score : {3}\nCalories Lost : {4}", score, coinsManager.GetCoinsNumber(), coinsManager.GetCoinValue(), totalScore, caloriesLost);

            if (caloriesToLost > 0 && caloriesLost >= caloriesToLost)
            {
                caloriesGoalText.enabled = true;
            }
        }
        else
        {
            deathMenuScore.text = System.String.Format("Distance : {0}m\nCoins : {1}\nMultiplier : {2}x\nTotal Score : {3}", score, coinsManager.GetCoinsNumber(), coinsManager.GetCoinValue(), totalScore);
        }

        int highScore = scoreManager.GetHighScore();
        if (totalScore > highScore)
        {
            UpdateHighScore(totalScore);
        }
    }

    /** 
     *  Updathe the highScore value
     */
    private void UpdateHighScore(int newHighScore)
    {
        highscoreSound.Play();
        PlayerPrefs.SetInt("HighScore", newHighScore);
        deathMenuHighScore.enabled = true;
    }

    /** 
     *  Restart the game by reset
     */
    public void Restart()
    {
        gameManager.Reset();
    }

    /** 
     *  Return to the main menu
     */
    public void BackToMainMenu()
    {
        gameManager.BackToMainMenu();
    }

    /** 
     *  Exit the game
     */
    public void Exit()
    {
        gameManager.ExitGame();
    }
}
