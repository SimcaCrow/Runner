using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private Text timeText;
    private GameObject timeManager;
    private CaloriesController caloriesManager;

    private float startingTime;
    private float actualTime;
    private float previousTime;

    private int caloriesToLost;
    private int timer;
    private bool isDead;

    /** 
     *  Initialization
     */
    void Start()
    {
        timeManager = GameObject.Find("TimeManager");
        caloriesManager = FindObjectOfType<CaloriesController>();

        caloriesToLost = PlayerPrefs.GetInt("CaloriesToLost");
        timer = PlayerPrefs.GetInt("Timer");
        previousTime = PlayerPrefs.GetInt("PreviousTime");
        isDead = false;

        if (timer == 0)
        {
            timeManager.SetActive(false);
        }
        else
        {
            startingTime = Time.time;
            timeText = GameObject.Find("TimeText").GetComponent<Text>();
        }
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        if (timer != 0 && !isDead)
        {
            UpdateTime();
            DisplayTime();
            ChangeTextColor();
        }
    }

    /** 
     *  Update the timer
     */
    private void UpdateTime()
    {
        actualTime = previousTime + (Time.time - startingTime);
    }

    /** 
     *  Display the timer on screen
     */
    private void DisplayTime()
    {
        int hours = (int)(actualTime / 3600);
        int minutes = (int)(actualTime / 60) - (60 * hours);
        int secondes = (int)(actualTime % 60);
        timeText.text = hours + ":" + ((minutes < 10) ? "0" + minutes.ToString() : minutes.ToString()) + ":" + ((secondes < 10) ? "0" + secondes.ToString() : secondes.ToString());
    }

    /** 
     *  Change the timer color according to your calorie and time goal
     */
    private void ChangeTextColor()
    {
        int caloriesLost = caloriesManager.GetCaloriesLost();
		if (actualTime <= (timer * 60) && caloriesLost >= caloriesToLost)
        {
            timeText.color = new Color(112 / 255f, 210 / 255f, 112 / 255f, 1);
        }
        else if (actualTime >= ((timer * 60) * 0.75f) && actualTime < (timer * 60) && caloriesLost < caloriesToLost)
        {
            timeText.color = new Color(253 / 255f, 182 / 255f, 27 / 255f, 1);
        }
        else if (actualTime >= (timer * 60))
        {
            timeText.color = new Color(239 / 255f, 84 / 255f, 79 / 255f, 1);
        }

    }

    /** 
     *  Fix the previous time done when the player die
     */
    public void FixPreviousTime()
    {
        previousTime += Time.time - startingTime;
        PlayerPrefs.SetInt("PreviousTime", (int)previousTime);
    }

    /** 
     *  Change the timer state when the player die or achieve his goal
     */
    public void SetIsDead(bool state)
    {
        isDead = state;
    }
}
