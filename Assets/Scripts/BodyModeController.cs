using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyModeController : MonoBehaviour
{
    private Text caloriesText;
    private Text timeText;
    private Text speedText;
    private Slider timeSlider;

    /** 
     *  Initialization
     */
    void Start()
    {
        caloriesText = GameObject.Find("CaloriesText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
        timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
    }

    /** 
     *	Set calorie text according to the calorie slider
     */
    public void SetCaloriesText(float value)
    {
        int calories = Mathf.RoundToInt(value);
        PlayerPrefs.SetInt("CaloriesToLost", calories);
        if (calories == 0)
        {
            caloriesText.text = "I want to 0 lose calorie";
            timeSlider.minValue = 0;
            timeSlider.maxValue = 0;
        }
        else
        {
            // Average mean = 30 kcal/5min (+/- 15 kcal) = 6 kcal/min (+/- 3 kcal)
            caloriesText.text = "I want to lose " + calories + " calories";
            timeSlider.minValue = Mathf.Round(calories / 9);
            timeSlider.value = Mathf.Round(calories / 6);
            timeSlider.maxValue = Mathf.Round(calories / 3);
        }
    }

    /** 
     *	Set time text according to the time slider
     */
    public void SetTimeText(float value)
    {
        int timer = Mathf.RoundToInt(value);
        int hours = timer / 60;
        int minutes = timer % 60;
        if (timer == timeSlider.minValue)
        {
            timeText.text = "Whitout timer";
            PlayerPrefs.SetInt("Timer", 0);
        }
        else
        {
            timeText.text = "Play for " + hours + ":" + ((minutes < 10) ? "0" + minutes.ToString() + ":00" : minutes.ToString() + ":00");
            PlayerPrefs.SetInt("Timer", timer);
        }
    }

    /** 
     *	Set speed text according to the speed slider
     */
    public void SetSpeedText(float value)
    {
        int speed = (int)value;
        PlayerPrefs.SetInt("Speed", speed);
        if (speed == 0)
        {
            speedText.text = "Normal";
        }
        else if (speed == 1)
        {
            speedText.text = "Hard";
        }
        else
        {
            speedText.text = "Extrem";
        }
    }
}
