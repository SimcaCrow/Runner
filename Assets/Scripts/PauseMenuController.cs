using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public AudioSource runSound;
    public AudioSource selectSound;

    private GameObject pauseMenu;
    private GameController gameManager;

    /** 
     *  Initialization
     */
    void Start()
    {
        gameManager = FindObjectOfType<GameController>();
        pauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
    }

    /** 
     *  Pause the game
     */
    public void Pause()
    {
        runSound.Pause();
        selectSound.Play();
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    /** 
     *  Resume the game
     */
    public void Resume()
    {
        selectSound.Play();
        runSound.Play();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    /** 
     *  Restart the game by reset
     */
    public void Restart()
    {
        Time.timeScale = 1f;
        gameManager.Reset();
    }

    /** 
     *  Return to main menu
     */
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
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
