using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Affdex;

public class GameController : MonoBehaviour
{
    public AudioSource selectSound;
    private GameObject deathMenu;
    private GameObject player;
    private GameObject pauseButton;
    private GameObject platformManager;

    /** 
     *  Initialization
     */
    void Start()
    {
        player = GameObject.Find("Player");
        pauseButton = GameObject.Find("PauseButton");
        deathMenu = GameObject.Find("Canvas").transform.Find("DeathMenu").gameObject;
        platformManager = GameObject.Find("PlatformGeneration");
        ModeSelection();
    }

    /** 
     *  Change the mode according to the player preferences
     */
    private void ModeSelection()
    {
        if (PlayerPrefs.GetInt("BodyMode") != 1)
        {
            GameObject.Find("CaloriesManager").SetActive(false);
            platformManager.GetComponent<PlatformsGenerationController>().enabled = false;

            if (PlayerPrefs.GetInt("TestMode") == 0)
                platformManager.GetComponent<PlatformsGenerationEmotionController>().enabled = true;
            //  WARNING : OBSOLETE, USED FOR THE 4 TEST LEVELS SYSTEM
            else
                platformManager.GetComponent<PlatformsGenerationPreExpController>().enabled = true;
            // WARNING : END

            Camera.main.gameObject.GetComponent<Detector>().enabled = true;
            Camera.main.gameObject.GetComponent<CameraInput>().enabled = true;
        }
    }

    /** 
     *  Charge the death menu after the player death
     */
    public void LoadDeathMenu()
    {
        player.SetActive(false);
        pauseButton.SetActive(false);
        deathMenu.SetActive(true);
    }

    /** 
     *  Return to the main menu
     */
    public void BackToMainMenu()
    {
        selectSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    /** 
     *  Reset the player position and restart the game
     */
    public void Reset()
    {
        selectSound.Play();
        SceneManager.LoadScene("Level");
    }

    /** 
     *  Exit the game
     */
    public void ExitGame()
    {
        selectSound.Play();
        Application.Quit();
    }
}
