using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesController : MonoBehaviour
{
    public AudioSource achievementSound;

    private TextFileController textManager;
    private Text caloriesText;
    private GameObject caloriesBar;

    private int caloriesLost;
    private int caloriesToLost;
    private bool soundHasBeingPlayed;

    /** 
     *  Initialization
     */
    void Start()
    {
        caloriesText = GameObject.Find("CaloriesText").GetComponent<Text>();
        textManager = FindObjectOfType<TextFileController>();
        caloriesBar = GameObject.Find("CaloriesBar");

        soundHasBeingPlayed = false;

        caloriesLost = textManager.CaloriesLost();
        caloriesToLost = PlayerPrefs.GetInt("CaloriesToLost");
        manageCalorieBar();
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        UpdateCalories();
        DisplayCalories();
        if (caloriesToLost > 0)
        {
            UpdateCaloriesBar();
            if ((caloriesToLost - caloriesLost) == 0 && !soundHasBeingPlayed)
            {
                achievementSound.Play();
                soundHasBeingPlayed = true;
            }
        }
    }

    /** 
     *  Change the state of the calorie bar according to the player goal
     */
    private void manageCalorieBar()
    {
        if (caloriesToLost == 0)
        {
            caloriesBar.SetActive(false);
        }
        else
        {
            caloriesBar.GetComponent<Slider>().maxValue = caloriesToLost;
        }
    }

    /** 
     *  Update the amount of calories lost
     */
    private void UpdateCalories()
    {
        caloriesLost = textManager.CaloriesLost();
    }

    /** 
     *  Display the number of calories lost on screen
     */
    private void DisplayCalories()
    {
        caloriesText.text = caloriesLost + ((caloriesLost == 0) ? " calorie" : " calories");
    }

    /** 
     *  Update the calorie bar on screen
     */
    private void UpdateCaloriesBar()
    {
        if ((caloriesToLost - caloriesLost) <= 0)
        {
            caloriesBar.GetComponent<Slider>().value = caloriesToLost;
        }
        else
        {
            caloriesBar.GetComponent<Slider>().value = caloriesLost;
        }
    }

    /** 
     *  Return the amount of calories lost
     */
    public int GetCaloriesLost()
    {
        return caloriesLost;
    }
}
