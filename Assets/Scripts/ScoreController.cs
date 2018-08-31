using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private Text scoreText;
    private GameObject player;
    private bool isPlayerAlive;
    private int score;

    /** 
     *  Initialization
     */
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        player = GameObject.Find("Player");

        isPlayerAlive = true;
        score = 0;
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        UpdateScore();
        DisplayScore();
    }

    /** 
     *  Update the player score
     */
    private void UpdateScore()
    {
        if (isPlayerAlive)
        {
            score = (int)player.GetComponent<Transform>().position.x;
        }
    }

    /** 
     *  Display the player score on screen
     */
    private void DisplayScore()
    {
        scoreText.text = score + "m";
    }

    /** 
     *  Return the current player score
     */
    public int GetScore()
    {
        return score;
    }

    /** 
     *  Return the current highscore
     */
    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            return PlayerPrefs.GetInt("HighScore");
        }
        return 0;
    }
}
